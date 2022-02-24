using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGrounded : State
{
    public StateGrounded(CharacterStateMachine context, StateFactory factory):base(context,factory){
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
            Debug.Log("ENTER GROUND");
        this.context.animator.Play(AnimationEnum.Idle.ToString());
        
    }

    public override void update()
    {
        this.checkSwitchStates();
    }

    public override void checkSwitchStates()
    {
        // is in ground, and jump was pressed
        if(this.context.isPressedJump){
            this.context.isPressedJump = false;
            this.switchState(this.factory.createJump());
        }
    }

    public override void exit() { }

}
