using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Factory

public class StateFactoryConserje: StateFactory{

    public StateFactoryConserje(CharacterStateMachine stateMachine):base(stateMachine){
        
    }

    public override State createGrounded()
    {
        return new StateConserjeGrounded(this.stateMachine, this);
    }

    public override State createJump()
    {
        return new StateConserjeJump(this.stateMachine, this);
    }


    public override State createIdle()
    {
        return new StateConserjeIdle(this.stateMachine, this);
    }
    public override State createWalk()
    {
        return new StateConserjeWalk(this.stateMachine, this);
    }
    public override State createPunch()
    {
        return new StateConserjePunch(this.stateMachine, this);
    }


}
    
#endregion


#region CustomStates

public class StateConserjeGrounded : StateGrounded
{
    public StateConserjeGrounded(CharacterStateMachine context, StateFactory factory):base(context,factory){

    }   
    // overrite methods if needed
}

public class StateConserjeJump : StateJump
{
    public StateConserjeJump(CharacterStateMachine context, StateFactory factory):base(context,factory){

    }
    // overrite methods if needed

}

public class StateConserjeIdle : StateIdle
{
    public StateConserjeIdle(CharacterStateMachine context, StateFactory factory):base(context,factory){}
    // overrite methods if needed
}

public class StateConserjeWalk : StateWalk
{
    public StateConserjeWalk(CharacterStateMachine context, StateFactory factory):base(context,factory){
        
    }
    // overrite methods if needed
}

public class StateConserjePunch : StatePunch
{
    public StateConserjePunch(CharacterStateMachine context, StateFactory factory):base(context,factory){

    }
}

#endregion


