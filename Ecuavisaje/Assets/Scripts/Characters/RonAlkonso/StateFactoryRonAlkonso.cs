using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFactoryRonAlkonso: StateFactory{

    public StateFactoryRonAlkonso(CharacterStateMachine stateMachine):base(stateMachine){
        
    }

    public override State createGrounded()
    {
        return new StateGroundedRonAlkonso(this.stateMachine, this);
    }

    public override State createJump()
    {
        return new StateJumpRonAlkonso(this.stateMachine, this);
    }

    public override State createIdle()
    {
        return new StateIdleRonAlkonso(this.stateMachine, this);
    }
    public override State createWalk()
    {
        return new StateWalkRonAlkonso(this.stateMachine, this);
    }
    public override State createPunch()
    {
        return new StatePunch1RonAlkonso(this.stateMachine, this);
    }
    public override State createSpecial1()
    {
        return new StateSpecial1RonAlkonso(this.stateMachine, this);
    }
    public override State createSpecial2()
    {
        return new StateSpecial2RonAlkonso(this.stateMachine, this);
    }
    public override State createUltimate()
    {
        return new StateUltimateRonAlkonso(this.stateMachine, this);
    }


}
    