using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateJump : State
{
    protected float timerJump;
    protected float timeInAir;
    protected float timeVelocityDownUpdate;
    protected Vector3 velocityJumpUp;
    protected Vector3 velocityJumpDown;

    public StateJump(CharacterStateMachine context, StateFactory factory):base(context,factory){
        this.timerJump = 0.3f;
        this.timeInAir = 0.4f;
        this.timeVelocityDownUpdate = 0.2f;
        this.isRootState = true;
        this.velocityJumpUp = new Vector3(0,this.context.velocityJump,0);
        this.velocityJumpDown = new Vector3(0,-this.context.velocityJump/2f,0);
        this.initSubState();
    }

    public override void initSubState()
    {
        if(this.context.isMoving){
            this.setStateSub(this.factory.createWalk());
        }
        else{
            this.setStateSub(this.factory.createIdle());
        }
    }

    public override void enter()
    {

        if(State.VERBOSE_LEVEL > 0)
            Debug.Log("ENTER JUMP");
        if(!this.context.isJumping){
            this.context.characterCommandGiver.cmdSetVelocity(this.velocityJumpUp * Time.deltaTime);
            this.context.isJumping = true;
            this.context.isPressedJump = false;
            this.context.animator.Play(AnimationEnum.Jump.ToString());
            
        }
        
    }

    public override void update()
    {
        this.checkSwitchStates();

        this.timerJump -= Time.deltaTime;
        // -1f is the reference
        if(this.context.isJumping && this.timerJump > -this.timeInAir && this.timerJump < 0){
            this.context.isJumping = false;
            // Smooth time in air
            this.context.characterCommandGiver.cmdSetVelocity(this.velocityJumpUp/4f * Time.deltaTime);
        }

        // reference of time in air contemplated in this loop counter
        if(this.timerJump < -this.timeInAir){
            this.timerJump = -this.timeInAir+this.timeVelocityDownUpdate;
            this.velocityJumpDown.y += -7f;
            Debug.Log("CALLING READJUST");
            this.context.characterCommandGiver.cmdSetVelocity(this.velocityJumpDown * Time.deltaTime);
        }
            
            

        

    }

    public override void checkSwitchStates()
    {
        if(!this.context.isJumping && this.context.isGrounded){
            this.context.characterCommandGiver.cmdSetVelocity(Vector3.zero);
            this.switchState(this.factory.createGrounded());
        }
    }

    public override void exit(){}

    private void handleGravity(){
        if(this.context.isGrounded){

        }
        else{

        }
    }

}
