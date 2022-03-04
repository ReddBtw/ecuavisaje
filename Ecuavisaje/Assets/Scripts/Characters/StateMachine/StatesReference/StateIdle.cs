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

        // todo: custom it
        if(this.context.isPressedPunch1 || this.context.isPressedPunch2){
            this.switchState(this.factory.createPunch());
        }


        if(this.context.isActivatedSpecial1){
            this.switchState(this.factory.createSpecial1());
        }
        else if(this.context.isActivatedSpecial2){
            this.switchState(this.factory.createSpecial2());
        }
        else if(this.context.isActivatedUltimate){
            this.switchState(this.factory.createUltimate());
        }

        
    }
    public override void exit(){}
}
