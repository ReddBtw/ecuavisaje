using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public static int VERBOSE_LEVEL = 0;
    protected bool isRootState = false;
    public CharacterStateMachine context;
    public StateFactory factory;
    protected State stateCurrentSub;
    protected State stateCurrentSuper;

    public State(CharacterStateMachine context, StateFactory factory){
        this.context = context;
        this.factory = factory;
    }


    // IEnumerator due to be invoked as coroutines
    public abstract void enter();
    public abstract void initSubState();
    public abstract void update();
    public abstract void checkSwitchStates();
    public abstract void exit();

    public void updateStates(){
        this.update();
        if(this.stateCurrentSub != null){
            this.stateCurrentSub.updateStates();
        }
    }

    protected void switchState(State stateNew){
        this.exit();

        stateNew.enter();
        
        if(this.isRootState){
            context.stateCurrent = stateNew;
        }
        else if(this.stateCurrentSuper != null){
            // set the current super state's sub state to the new state
            this.stateCurrentSuper.setStateSub(stateNew);
        }
    }

    protected void setStateSuper(State stateNewSuper){
        this.stateCurrentSuper = stateNewSuper;
    }

    protected void setStateSub(State stateNewSub){
        // children and parent meet each other
        this.stateCurrentSub = stateNewSub;
        stateNewSub.setStateSuper(this);
    }


    
}
