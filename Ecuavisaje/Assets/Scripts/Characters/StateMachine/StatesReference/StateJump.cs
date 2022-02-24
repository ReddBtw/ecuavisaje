using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateJump : State
{
    protected float timerJump;

    public StateJump(CharacterStateMachine context, StateFactory factory):base(context,factory){
        this.timerJump = 1f;
        this.isRootState = true;
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


            this.context.cmdJump(this.context.forceJump);
            this.context.isJumping = true;
            this.context.isPressedJump = false;
            this.context.animator.Play(AnimationEnum.Jump.ToString());
            
        }
    }


    public override void update()
    {
        this.timerJump -= Time.deltaTime;
        if(this.timerJump < 0){
            this.context.isJumping = false;
        }

        this.checkSwitchStates();
    }

    public override void checkSwitchStates()
    {
        if(!this.context.isJumping && this.context.isGrounded){
            this.switchState(this.factory.createGrounded());
        }
    }

    public override void exit(){}
}
