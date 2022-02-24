using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : State
{
    public StateIdle(CharacterStateMachine context, StateFactory factory):base(context,factory){
        
    }

    public override void initSubState(){

    }
    public override void enter()
    {
        if(State.VERBOSE_LEVEL > 0)
            Debug.Log("ENTER IDLE");
        this.context.animator.Play(AnimationEnum.Idle.ToString());
    }



    public override void update()
    {
        this.checkSwitchStates();
    }

    public override void checkSwitchStates()
    {

        if(this.context.isMoving){
            this.switchState(this.factory.createWalk());
        }

        if(this.context.isPressedPunch1){
            this.switchState(this.factory.createPunch());
        }

        
    }
    public override void exit(){}
}
