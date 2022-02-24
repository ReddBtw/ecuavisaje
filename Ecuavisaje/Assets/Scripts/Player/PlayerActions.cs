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
        Debug.Log("PLAYER: " + this.player.id);
        
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
        this.inputActions = new InputActions();
        this.inputActions.Player.Enable();
        this.inputActions.Player.Jump.performed += this.jump;
        this.side = this.side_last;
        this.animator = this.GetComponent<Animator>();
        this.rb = this.GetComponent<Rigidbody>();
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
            Debug.Log("Player actions:Update:LOOK AT");
            this.cmdLookAt(this.player.opponent.transform.position.x);      
        }
        this.side_last = this.side;
        
        
        float left = inputActions.Player.WalkLeft.ReadValue<float>();
        float right = inputActions.Player.WalkRight.ReadValue<float>();
        
        if(left > 0){
            this.cmdMoveX(-this.speedWalk * Time.deltaTime);
            this.animator.SetBool("is_walking", true);
        }
        else if(right > 0){
            this.cmdMoveX(this.speedWalk * Time.deltaTime);
            this.animator.SetBool("is_walking", true);
        }
        else{
            this.animator.SetBool("is_walking", false);
        }
        
        
    }


    private void jump(InputAction.CallbackContext context){
        if(!hasAuthority) return;
        this.cmdJump(this.forceJump);
    }

    private void bend(InputAction.CallbackContext context){
        if(!hasAuthority) return;
    }

    private void punch1(InputAction.CallbackContext context){
        if(!hasAuthority) return;
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

    private void block(InputAction.CallbackContext context){
        if(!hasAuthority) return;
    }


}
