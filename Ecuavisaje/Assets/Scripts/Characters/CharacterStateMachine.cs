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
    public bool isPressedBend = false;
    public bool isPressedLeft = false;
    public bool isPressedRight = false;
    public bool isMoving = false;
    public bool isJumping = false;
    public bool isAttacking = false;
    public bool isPressedPunch1 = false;
    public bool isPressedPunch2 = false;

    
    public Animator animator {get;set;}
    public CharacterCommandGiver characterCommandGiver {get; set;}
    public Health health {get; set;}

    private List<Character> characters;  
    
    [Header("Character config")]
    [SerializeField] private CharacterEnum characterEnum = CharacterEnum.None;
    public Character characterSelected {get;set;}

    [Header("Physics")]
    [SerializeField]
    public float speedWalk = 12f;
    [SerializeField]
    public float forceJump = 400f;

    #region GettersSetters

    public CharacterEnum getCharacterEnum(){ return this.characterEnum; }
        
    #endregion

    #region Server and Client callbacks
    public override void OnStartServer(){

        if(!NetworkServer.active) return;


        foreach(GameObject gameObjectPlayer in GameObject.FindGameObjectsWithTag("Player")){
            NetworkPlayer p = gameObjectPlayer.GetComponent<NetworkPlayer>();
            
            this.player = p;
            
        }
        
    }

    public override void OnStartClient()
    {
        if(!hasAuthority) return;

        this.inputActions = new InputActions();
        this.inputActions.Player.Enable();
        this.inputActions.Player.Jump.performed += this.jump;
        this.inputActions.Player.Punch1.performed += this.punch1;
        this.inputActions.Player.Punch2.performed += this.punch2;

        this.player = NetworkClient.connection.identity.GetComponent<NetworkPlayer>();

        Debug.Log("Net id: " + this.player.netId + ". Has: " + this.player.hasAuthority);
    }

     public override void OnStopClient()
    {
        if(!hasAuthority) return;

        this.inputActions.Player.Disable();
        this.inputActions.Player.Jump.performed -= this.jump;
        this.inputActions.Player.Punch1.performed -= this.punch1;

        
    }
    #endregion


    void Start(){

        this.characters = UtilsResources.getScriptableObjectsCharacters();

        foreach (Character character in this.characters)
        {
            if(this.characterEnum == character.characterEnum){
                this.characterSelected = character;
            }
        }

        this.characterCommandGiver = this.GetComponent<CharacterCommandGiver>();
        this.health = this.GetComponent<Health>();
        this.animator = this.GetComponent<Animator>();
        this.side = 0;
        this.side_last = -1;
        this.side = this.side_last;
        
        this.groundMask = 1 << LayerMask.NameToLayer("Ground");

        switch (this.characterEnum)
        {
            case CharacterEnum.Lasso:
                this.stateFactory = new StateFactoryLasso(this);
                break;
            case CharacterEnum.Conserje:
                this.stateFactory = new StateFactoryConserje(this);
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

        float bend = inputActions.Player.Bend.ReadValue<float>();
        if(bend > 0){
            this.isPressedBend = true;
        }
        else{
            this.isPressedBend = false;
        }
    }


    [ClientCallback]
    private void FixedUpdate() {
        RaycastHit raycastHit;
        if(Physics.Raycast(this.transform.position, Vector3.down, out raycastHit, 0.5f, this.groundMask)){
            isGrounded = true;
        }
        else{
            isGrounded = false;
        }
    }


    private void jump(InputAction.CallbackContext context){
        if(!hasAuthority) return;
        if(!this.isJumping){

            this.isPressedJump = true;
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

    private void findOpponent(){
        foreach (GameObject gameObjectPlayer in GameObject.FindGameObjectsWithTag("Character"))
        {
            if(!GameObject.ReferenceEquals(this.gameObject, gameObjectPlayer)){
                this.player.opponent = gameObjectPlayer;
            }
        }
    }    
}
