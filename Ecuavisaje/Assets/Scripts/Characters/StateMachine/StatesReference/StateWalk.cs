using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateWalk : State
{
    public StateWalk(CharacterStateMachine context, StateFactory factory):base(context,factory){
        
    }

   
    public override void initSubState(){}

    public override void enter()
    {
        if(State.VERBOSE_LEVEL > 0)
            Debug.Log("ENTER Walk");
        if(!this.context.isJumping)
            this.context.animator.Play(AnimationEnum.Walk.ToString());
        
    }


    public override void update()
    {
        this.checkSwitchStates();
        if(this.context.isPressedLeft){
            this.context.cmdMoveX(-this.context.speedWalk * Time.deltaTime);
        }
        else if(this.context.isPressedRight){
            this.context.cmdMoveX(this.context.speedWalk * Time.deltaTime);
        }
    }

     public override void checkSwitchStates()
    {
        
        if(!this.context.isMoving){
            this.switchState(this.factory.createIdle());
        }

        if(this.context.isPressedPunch1){
            this.switchState(this.factory.createPunch());
        }
    }
    public override void exit(){}
}
