using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateFactory
{
    public CharacterStateMachine stateMachine;

    public StateFactory(CharacterStateMachine stateMachine){
        this.stateMachine = stateMachine;
    }
    public abstract State createGrounded();
    public abstract State createJump();
    public abstract State createIdle();
    public abstract State createWalk();
    public abstract State createPunch();
    public abstract State createSpecial1();
    public abstract State createSpecial2();
    public abstract State createUltimate();

    
}
