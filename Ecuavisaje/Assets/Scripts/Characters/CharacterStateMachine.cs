using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using System;
public enum AnimationEnum
{
    Idle, WalkForward, WalkBackward, Jump, Bend, Sweep, Punch1, Punch2,
    Kick1, Kick2, Block1, GettingUp, KnockedOut,
    ReceiveDamageUp, Stunned, Special1, Special2,
    Ultimate, Uppercut
}

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(NetworkAnimator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class CharacterStateMachine : NetworkBehaviour
{
    [SyncVar(hook=nameof(handleUpdatedHealth))]
    private int health = 100;

    public State stateCurrent {get;set;}
    public StateFactory stateFactory {get;set;}

    public Animator animator {get;set;}
    private Rigidbody rb;
    public  AudioSource audioSource {get;set;}   
    [SerializeField] AudioClip dump;
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

    

    
    [Header("Character config")]
    [SerializeField] private CharacterEnum characterEnum = CharacterEnum.None;
    [SerializeField] private GameObject pointHitPunch;

    [Header("Physics")]
    [SerializeField]
    public float speedWalk = 12f;
    [SerializeField]
    public float forceJump = 400f;

    #region GettersSetters

    public CharacterEnum getCharacterEnum(){
        return this.characterEnum;
    }

    public GameObject getPointHitPunch(){
        return this.pointHitPunch;
    }
    
        
    #endregion


    #region Server
    public override void OnStartServer(){



        if(!NetworkServer.active) return;


        

        foreach(GameObject gameObjectPlayer in GameObject.FindGameObjectsWithTag("Player")){
            NetworkPlayer p = gameObjectPlayer.GetComponent<NetworkPlayer>();
            
            this.player = p;
            
        }

        

        /*
        foreach(Character c in this.player.getCharacters()){
            if(c.characterEnum == this.characterEnum){
                this.characterSelected = c;
                Debug.Log("Character selected: " + c.characterEnum);
                break;
            }
        }
        */

        
        
    }        
    #endregion

    #region Client
    public override void OnStartClient()
    {
        if(!hasAuthority) return;

        this.inputActions = new InputActions();
        this.inputActions.Player.Enable();
        this.inputActions.Player.Jump.performed += this.jump;
        this.inputActions.Player.Punch1.performed += this.punch1;

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


    void Start(){
        this.rb = this.GetComponent<Rigidbody>();
        this.animator = this.GetComponent<Animator>();
        this.audioSource = this.GetComponent<AudioSource>();
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
        
        if(this.pointHitPunch != null){
            Debug.DrawLine(this.transform.position, this.pointHitPunch.transform.position, Color.blue);
        }
        
        this.checkLookDirection();
        this.readInput();
        this.stateCurrent.updateStates();        
    }

    private void checkLookDirection() {
        this.side = (this.transform.position.x < this.player.opponent.transform.position.x)? 0:1;

        if(this.side != this.side_last){
            // todo: check if this can be optimized
            this.cmdLookAt(this.player.opponent.transform.position.x);      
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

    private void findOpponent(){
        foreach (GameObject gameObjectPlayer in GameObject.FindGameObjectsWithTag("Character"))
        {
            if(!GameObject.ReferenceEquals(this.gameObject, gameObjectPlayer)){
                this.player.opponent = gameObjectPlayer;
            }
        }
    }

    public void handleUpdatedHealth(int valueOld, int valueNew){
        this.health = valueNew;
        if(this.animator != null){
            StartCoroutine(receiveDamage(AnimationEnum.ReceiveDamageUp));
            
        }
    }

    IEnumerator receiveDamage(AnimationEnum animationEnum){
        this.animator.Play(animationEnum.ToString());
        yield return new WaitForSeconds(1f);
        this.animator.Play(AnimationEnum.Idle.ToString());
    }



    #endregion

    

    #region Command
    
    [Command]
    public void cmdMoveX(float x){
        this.transform.Translate(x, 0, 0, Space.World);
    }


    [Command]
    public void cmdJump(float force){
        this.rb.AddForce(new Vector3(0,force,0), ForceMode.Force);
    }

    [Command]
    public void cmdLookAt(float x){
        
        this.transform.LookAt(new Vector3(
            x,
            this.transform.position.y,
            this.transform.position.z
        ));
    }

    [Command]
    public void cmdPlaySound(CharacterEnum characterEnum, CharacterAudioEnum characterAudioEnum){
        this.rpcPlayAudio(characterEnum, characterAudioEnum);
    }

    [Command]
    public void cmdAttackPunch(float attackRange, int layer){
        Collider[] hits = Physics.OverlapSphere(this.getPointHitPunch().transform.position, attackRange, layer);

        foreach (Collider hit in hits)
        {
            CharacterStateMachine h = hit.gameObject.GetComponent<CharacterStateMachine>();
            if(h.connectionToClient.connectionId != this.connectionToClient.connectionId){
                Debug.Log("Enemy: " + h.gameObject.name);
                h.health -= 10;
            }
            
        }
        Debug.Log("HITS: " + hits.Length);
        if(hits.Length == 1){

        }
    }

    #endregion

    #region RPC

    [ClientRpc]
    public void rpcPlayAudio(CharacterEnum characterEnum, CharacterAudioEnum characterAudioEnum){
        // todo: optimice with hashing?

        this.audioSource.PlayOneShot(this.dump);

        /*
        foreach (Character character in this.player.getCharacters())
        {
            if(character.characterEnum == characterEnum){
                this.audioSource.PlayOneShot(character.dump);
                
                foreach (CharacterAudio characterAudio in character.audios)
                {
                    if(characterAudio.characterAudioEnum == characterAudioEnum){
                        Debug.Log($"Audio {characterAudio.characterAudioEnum} {characterAudio.audio}");
                        // this.audioSource.PlayOneShot(characterAudio.audio);
                        break;
                    }
                }
                
            }            
        }
        */
    }
        
    #endregion

}
