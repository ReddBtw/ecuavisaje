using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerActions: NetworkBehaviour
{
    [SerializeField]
    public float speedWalk = 10f;
    [SerializeField]
    public float forceJump = 20f;
    private Animator animator;
    private Rigidbody rb;
   
    private InputActions inputActions;
    private int side;
    private int side_last;
    private NetworkPlayer player;

    private int groundMask;

    public bool is_walking {get;set;} = false;
    public bool is_idle {get;set;} = true;
    public bool is_jump_pressed  {get;set;} = false;
    public bool is_grounded  {get;set;} = false;
    public bool is_bend_pressed  {get;set;} = false;
    public bool is_attacking  {get;set;} = false;
    public bool is_punch1_pressed  {get;set;} = false;



    

    public Animator getAnimator(){
        return this.animator;
    }

    #region Server

    public override void OnStartServer()
    {
        
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
    public void cmdBend(){
        // todo: implement bend collision changer
    }


    [Command]
    public void cmdLookAt(float x){
        
        this.transform.LookAt(new Vector3(
            x,
            this.transform.position.y,
            this.transform.position.z
        ));
    }

    #endregion
    
    public override void OnStartClient()
    {
        if(!hasAuthority) return;

        this.player = NetworkClient.connection.identity.GetComponent<NetworkPlayer>();

        this.inputActions = new InputActions();
        this.inputActions.Player.Enable();
        this.inputActions.Player.Jump.performed += this.jump;
        this.inputActions.Player.Punch1.performed += this.punch1;

        
    }

    public override void OnStopClient()
    {
        if(!hasAuthority) return;

        this.inputActions.Player.Disable();
        this.inputActions.Player.Jump.performed -= this.jump;
        this.inputActions.Player.Punch1.performed -= this.punch1;

        
    }

    private void findOpponent(){
        foreach (GameObject gameObjectPlayer in GameObject.FindGameObjectsWithTag("Character"))
        {
            if(!GameObject.ReferenceEquals(this.gameObject, gameObjectPlayer)){
                this.player.opponent = gameObjectPlayer;
            }
        }
    }

    void Start(){
        this.side = 0;
        this.side_last = -1;
        this.side = this.side_last;
        this.animator = this.GetComponent<Animator>();
        this.rb = this.GetComponent<Rigidbody>();
        this.groundMask = 1 << LayerMask.NameToLayer("Ground");
    }


    private void Update()
    {
        if(!hasAuthority) return;

        if(this.player.opponent == null){
            // todo: optimize
            this.findOpponent();
            return;
        }


        this.side = (this.transform.position.x < this.player.opponent.transform.position.x)? 0:1;

        if(this.side != this.side_last){
            // todo: check if this can be optimized
            this.cmdLookAt(this.player.opponent.transform.position.x);      
        }
        this.side_last = this.side;
        
        
        float left = inputActions.Player.WalkLeft.ReadValue<float>();
        float right = inputActions.Player.WalkRight.ReadValue<float>();
        
        if(left > 0){
            this.cmdMoveX(-this.speedWalk * Time.deltaTime);
            this.is_walking = true;
        }
        else if(right > 0){
            this.cmdMoveX(this.speedWalk * Time.deltaTime);
            this.is_walking = true;
        }
        else{
            this.is_walking = false;
        }

        float bend = inputActions.Player.Bend.ReadValue<float>();
        if(bend > 0){
            this.is_bend_pressed = true;
        }
        else{
            this.is_bend_pressed = false;
        }
        
        
    }

    void FixedUpdate(){
        RaycastHit raycastHit;
        if(Physics.Raycast(this.transform.position, Vector3.down, out raycastHit, 1f, this.groundMask)){
            is_grounded = true;
        }
        else{
            is_grounded = false;
        }
        // Debug.Log(is_grounded);
    }


    private void jump(InputAction.CallbackContext context){
        if(!hasAuthority) return;
        this.is_jump_pressed = true;
        this.cmdJump(this.forceJump);
    }

    private void punch1(InputAction.CallbackContext context){
        if(!hasAuthority) return;
        this.is_attacking = true;
        this.is_punch1_pressed = true;
        // this.cmdPunch1();
    }

    private void punch2(InputAction.CallbackContext context){
        if(!hasAuthority) return;
    }

    private void kick1(InputAction.CallbackContext context){
        if(!hasAuthority) return;
    }

    private void kick2(InputAction.CallbackContext context){
        if(!hasAuthority) return;
    }



}
