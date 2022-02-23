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
    private Player player;

    private float timer_base_look_to = 0.4f;
    private float timer_counter_look_to = 0f;

    void Start(){
        this.side = 0;
        this.side_last = -1;
        this.rb = GetComponent<Rigidbody>();
        this.animator = GetComponent<Animator>();
        this.inputActions = new InputActions();
        this.inputActions.Player.Enable();
        this.inputActions.Player.Jump.performed += this.jump;
        this.side = this.side_last;

        

    }

    public override void OnStartServer()
    {
        this.player = this.connectionToClient.identity.GetComponent<Player>();
    }
    public override void OnStartClient()
    {
        if(!hasAuthority) return;
        if(!isClientOnly) return; 

        this.player = NetworkClient.connection.identity.GetComponent<Player>();
        
        /*
        List<Player> players = ((EcuavisajeNetworkManager)NetworkManager.singleton).players;

        int id_self = this.player.id;
        // todo: remove debug messages
        Debug.Log($"OnStartClient Client{id_self} {this.player.transform.position}, users: {players.Count}");

        foreach (Player p in players)
        {
            int id_player = p.id;
            if(id_player != id_self){
                this.player.opponent = p.gameObject;
                Debug.Log($"SELECTED Player {id_player}: {this.player.opponent.transform.position}");
                break;
            }
            else{
                Debug.Log($"DISCARD Player: {id_player}");
            }
            
        }
        */
        
    }

    [Command]
    public void cmdMoveX(float x){
        this.transform.Translate(x, 0, 0, Space.World);
        
    }


    [Command]
    public void cmdJump(float force){
        this.rb.AddForce(new Vector3(0,force,0), ForceMode.Force);
        
    }


    [Command]
    public void cmdLookAt(){
        
        this.transform.LookAt(new Vector3(
            this.player.opponent.transform.position.x,
            this.transform.position.y,
            this.transform.position.z
        ));
    }





    [ClientCallback]
    private void Update()
    {
        if(!hasAuthority) return;


        this.timer_counter_look_to += Time.deltaTime;
        if(this.timer_counter_look_to > this.timer_base_look_to){
            // todo: check if this can be optimized
            this.cmdLookAt();
            this.timer_counter_look_to = 0f;            
        }
        
        
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
