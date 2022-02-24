using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateFactory
{
    public StateMachine stateMachine;

    public StateFactory(StateMachine stateMachine){
        this.stateMachine = stateMachine;
    }
    public abstract State createGrounded();
    public abstract State createJump();
    public abstract State createIdle();
    public abstract State createWalk();
    public abstract State createPunch1();
}
