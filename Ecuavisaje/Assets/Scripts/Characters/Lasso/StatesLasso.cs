using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Factory

public class StateFactoryLasso: StateFactory{

    public StateFactoryLasso(CharacterStateMachine stateMachine):base(stateMachine){
        
    }

    public override State createGrounded()
    {
        return new StateLassoGrounded(this.stateMachine, this);
    }

    public override State createJump()
    {
        return new StateLassoJump(this.stateMachine, this);
    }


    public override State createIdle()
    {
        return new StateLassoIdle(this.stateMachine, this);
    }
    public override State createWalk()
    {
        return new StateLassoWalk(this.stateMachine, this);
    }
    public override State createPunch()
    {
        return new StateLassoPunch(this.stateMachine, this);
    }
    public override State createSpecial1()
    {
        return new StateLassoSpecial1(this.stateMachine, this);
    }


}
    
#endregion


#region CustomStates

public class StateLassoGrounded : StateGrounded
{
    public StateLassoGrounded(CharacterStateMachine context, StateFactory factory):base(context,factory){

    }   
    // overrite methods if needed
}

public class StateLassoJump : StateJump
{
    public StateLassoJump(CharacterStateMachine context, StateFactory factory):base(context,factory){

    }
    // overrite methods if needed

}

public class StateLassoIdle : StateIdle
{
    public StateLassoIdle(CharacterStateMachine context, StateFactory factory):base(context,factory){}
    // overrite methods if needed
}

public class StateLassoWalk : StateWalk
{
    public StateLassoWalk(CharacterStateMachine context, StateFactory factory):base(context,factory){
        
    }
    // overrite methods if needed
}

public class StateLassoPunch : StatePunch
{
    public StateLassoPunch(CharacterStateMachine context, StateFactory factory):base(context,factory){

    }
}


public class StateLassoSpecial1 : State
{
    public float timerDuration = 1f;
    protected LayerMask layerPlayer;

    public StateLassoSpecial1(CharacterStateMachine context, StateFactory factory):base(context,factory){
        this.layerPlayer = 1 << LayerMask.NameToLayer("Entity");
    }

    public override void enter()
    {
        Debug.Log("ENTER SPECIAL1");
        this.context.isPressedPunch2 = false;
        this.context.cmdPlaySound(this.context.getCharacterEnum(), CharacterAudioEnum.Punch);
        this.context.animator.Play(AnimationEnum.Special1.ToString());
        this.context.cmdInvokeSpecial1(this.context.getCharacterEnum());

    }

    public override void exit(){}

    public override void initSubState(){}

    public override void update()
    {
        timerDuration -= Time.deltaTime;
        if(timerDuration < 0){
            this.context.isAttacking = false;
        }
        this.checkSwitchStates();
    }

    public override void checkSwitchStates()
    {
        if(!this.context.isAttacking){
            if(this.context.isMoving){
                this.switchState(this.factory.createWalk());
            }
            else{
                this.switchState(this.factory.createIdle());
            }
        }
        
        
    }
}


#endregion


