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

#endregion


