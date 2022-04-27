using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Mirror;
using System;


[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(NetworkAnimator))]
[RequireComponent(typeof(CharacterCommandGiver))]
[RequireComponent(typeof(Health))]
public class CharacterStateMachine : NetworkBehaviour
{
    public State stateCurrent {get;set;}
    public StateFactory stateFactory {get;set;}

    
    private InputActions inputActions;
    public int side {get;set;}
    private int side_last;
    private NetworkPlayer player;

    private int groundMask;

    public bool isGrounded = true;
    public bool isPressedJump = false;
    public bool isPressedLeft = false;
    public bool isPressedRight = false;
    public bool isMoving = false;
    public bool isJumping = false;
    public bool isBending = false;
    public bool isAttacking = false;
    public bool isPressedPunch1 = false;
    public bool isPressedPunch2 = false;

    public bool isActivatedSpecial1 = false;
    public bool isActivatedSpecial2 = false;
    public bool isActivatedUltimate = false;



 
    
    public Animator animator {get;set;}
    public CharacterCommandGiver characterCommandGiver {get; set;}
    public Health health {get; set;}

    public CutSceneController cutSceneController {get; set;}

    public List<Character> characters;
    
    [Header("Character config")]
    [SerializeField] private CharacterEnum characterEnum = CharacterEnum.None;
    public Character characterSelected {get;set;}

    [Header("Physics")]
    [SerializeField]
    public float speedWalk = 12f;
    [SerializeField]
    public float velocityJump = 1f;

    #region GettersSetters

    public CharacterEnum getCharacterEnum(){ return this.characterEnum; }
        
    #endregion

    #region Server and Client callbacks
    public override void OnStartServer(){

        foreach(GameObject gameObjectPlayer in GameObject.FindGameObjectsWithTag("Player")){
            NetworkPlayer p = gameObjectPlayer.GetComponent<NetworkPlayer>();
            
            if(p.hasAuthority){
                this.player = p;
                Debug.Log($"Player in start server: auth={this.player.hasAuthority}, conn={this.player.connectionToClient}");
            
            }

            
        }


        if(!NetworkServer.active) return;


        
        
    }

    public override void OnStartClient()
    {

        if(!hasAuthority){
            // execute in others locally
            // this.gameObject.layer = LayerMask.NameToLayer("Enemy");
            return;
        }


        this.inputActions = new InputActions();
        this.inputActions.Player.Enable();

        this.inputActions.Player.Jump.performed += this.onJump;
        this.inputActions.Player.Bend.performed += this.onBend;

        this.inputActions.Player.Punch1.performed += this.punch1;
        this.inputActions.Player.Punch2.performed += this.punch2;
        this.inputActions.Player.DebugSpecial1.performed += this.debugSpecial1;
        this.inputActions.Player.DebugSpecial2.performed += this.debugSpecial2;
        this.inputActions.Player.DebugUltimate.performed += this.debugUltimate;

        this.player = NetworkClient.connection.identity.GetComponent<NetworkPlayer>();

        this.gameObject.layer = LayerMask.NameToLayer(this.player.team_layer);

        // Debug.Log("Net id: " + this.player.netId + ". Has: " + this.player.hasAuthority);
    }

     public override void OnStopClient()
    {
        if(!hasAuthority) return;

        this.inputActions.Player.Disable();
        this.inputActions.Player.Jump.performed -= this.onJump;
        this.inputActions.Player.Bend.performed -= this.onBend;

        this.inputActions.Player.Punch1.performed -= this.punch1;
        this.inputActions.Player.Punch2.performed -= this.punch2;
        this.inputActions.Player.DebugSpecial1.performed -= this.debugSpecial1;
        this.inputActions.Player.DebugSpecial2.performed -= this.debugSpecial2;
        this.inputActions.Player.DebugUltimate.performed -= this.debugUltimate;

        
    }
    #endregion


    void Start(){

        this.characterCommandGiver = this.GetComponent<CharacterCommandGiver>();
        this.health = this.GetComponent<Health>();
        this.animator = this.GetComponent<Animator>();
        this.cutSceneController = GameObject.FindObjectOfType<CutSceneController>();


        this.characters = UtilsResources.getScriptableObjectsCharacters();

        foreach (Character character in this.characters)
        {
            if(this.characterEnum == character.characterEnum){
                this.characterSelected = character;
            }
        }

        this.side = 0;
        this.side_last = -1;
        this.side = this.side_last;
        
        this.groundMask = 1 << LayerMask.NameToLayer("Ground");


        if(!hasAuthority) return;

        switch (this.characterEnum)
        {
            case CharacterEnum.Lasso:
                this.stateFactory = new StateFactoryLasso(this);
                break;
            case CharacterEnum.Conserje:
                this.stateFactory = new StateFactoryConserje(this);
                break;
            case CharacterEnum.RonAlkonso:
                this.stateFactory = new StateFactoryRonAlkonso(this);
                break;
            default:
                throw new System.Exception("CharacterEnum invalid");
        }

        
        this.stateCurrent = this.stateFactory.createGrounded();
        this.stateCurrent.enter();

        


    }

    [ClientCallback]
    void Update()
    {
        if(!hasAuthority) return;

        if(this.player.opponent == null){
            // todo: optimize
            this.findOpponent();
            return;
        }

        // Vector3 point = new Vector3(this.transform.position.x+1, this.transform.position.y, this.transform.position.z);

        this.checkLookDirection();
        this.readInput();
        this.stateCurrent.updateStates();        
    }

    private void checkLookDirection() {
        this.side = (this.transform.position.x < this.player.opponent.transform.position.x)? 0:1;

        if(this.side != this.side_last){
            // todo: check if this can be optimized
            this.characterCommandGiver.cmdLookAt(this.player.opponent.transform.position.x);      
        }
        this.side_last = this.side;
    }

    private void readInput()
    {
        float left = inputActions.Player.WalkLeft.ReadValue<float>();
        float right = inputActions.Player.WalkRight.ReadValue<float>();
        
        if(left > 0){
            // mutual exclusive
            this.isPressedLeft = true;
            this.isPressedRight = false;
        }
        else if(right > 0){
            // mutual exclusive
            this.isPressedRight = true;
            this.isPressedLeft = false;
        }
        else{
            this.isPressedRight = false;
            this.isPressedLeft = false;
        }

        this.isMoving = this.isPressedLeft || this.isPressedRight;

    }


    [ClientCallback]
    private void FixedUpdate() {
        if(!hasAuthority) return;
        RaycastHit raycastHit;
        if(Physics.Raycast(this.transform.position, Vector3.down, out raycastHit, 0.5f, this.groundMask)){
            isGrounded = true;
        }
        else{
            isGrounded = false;
        }
    }


    private void onJump(InputAction.CallbackContext context){
        if(!hasAuthority) return;

        // Debug.Log("PRESS JUMO: " + this.isPressedJump);
        
        if(!this.isJumping){

            this.isPressedJump = true; // context.ReadValueAsButton();
        }
        
    }

    private void onBend(InputAction.CallbackContext context){
        if(!hasAuthority) return;
        
        if(!this.isBending && this.isGrounded){
            this.isBending = true; // context.ReadValueAsButton();
        }
        
    }

    

    private void punch1(InputAction.CallbackContext context){
        if(!hasAuthority) return;
        this.isAttacking = true;
        this.isPressedPunch1 = true;
    }

    private void punch2(InputAction.CallbackContext context){
        if(!hasAuthority) return;
        this.isAttacking = true;
        this.isPressedPunch2 = true;
    }

    // todo: control this when go to production
    private void debugSpecial1(InputAction.CallbackContext context){
        if(!hasAuthority) return;
        //1
        this.isActivatedSpecial1 = true;
    }
    private void debugSpecial2(InputAction.CallbackContext context){
        if(!hasAuthority) return;
        //2
        this.isActivatedSpecial2 = true;
    }
    private void debugUltimate(InputAction.CallbackContext context){
        if(!hasAuthority) return;
        //3
        this.isActivatedUltimate = true;
    }

    private void findOpponent(){
        foreach (GameObject gameObjectPlayer in GameObject.FindGameObjectsWithTag("Character"))
        {
            if(!GameObject.ReferenceEquals(this.gameObject, gameObjectPlayer)){
                this.player.opponent = gameObjectPlayer;
            }
        }
    }    
}
