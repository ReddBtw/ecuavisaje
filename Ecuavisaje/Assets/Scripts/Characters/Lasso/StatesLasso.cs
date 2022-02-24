using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StateFactoryLasso: StateFactory{

    public StateFactoryLasso(StateMachine stateMachine):base(stateMachine){
        
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
    public override State createPunch1()
    {
        return new StateLassoPunch1(this.stateMachine, this);
    }


}

public class StateLassoGrounded : State
{
    public StateLassoGrounded(StateMachine context, StateFactory factory):base(context,factory){
        this.isRootState = true;
        this.initSubState();
    }

    public override void initSubState()
    {
        
        if(this.context.isMoving){
            this.setStateSub(this.factory.createWalk());
        }
        else{
            this.setStateSub(this.factory.createIdle());
        }
    }


    public override void enter()
    {
        Debug.Log("ENTER GROUND");
        this.context.animator.Play(AnimationEnum.Idle.ToString());
        
    }

    public override void update()
    {
        this.checkSwitchStates();
    }

    public override void checkSwitchStates()
    {
        // is in ground, and jump was pressed
        if(this.context.isPressedJump){
            this.context.isPressedJump = false;
            this.switchState(this.factory.createJump());
        }
    }

    public override void exit() { }

    
    
}

public class StateLassoJump : State
{
    private float timerJump;

    public StateLassoJump(StateMachine context, StateFactory factory):base(context,factory){
        this.timerJump = 1f;
        this.isRootState = true;
        this.initSubState();
    }

    public override void initSubState()
    {
        if(this.context.isMoving){
            this.setStateSub(this.factory.createWalk());
        }
        else{
            this.setStateSub(this.factory.createIdle());
        }
    }

    public override void enter()
    {

        Debug.Log("ENTER JUMP");

        if(!this.context.isJumping){


            this.context.cmdJump(this.context.forceJump);
            this.context.isJumping = true;
            this.context.isPressedJump = false;
            this.context.animator.Play(AnimationEnum.Jump.ToString());
            
        }
        

    }

    public override void update()
    {
        this.timerJump -= Time.deltaTime;
        if(this.timerJump < 0){
            this.context.isJumping = false;
        }

        this.checkSwitchStates();
    }

    public override void checkSwitchStates()
    {
        if(!this.context.isJumping && this.context.isGrounded){
            this.switchState(this.factory.createGrounded());
        }
    }

    public override void exit(){}
}

public class StateLassoIdle : State
{

    public StateLassoIdle(StateMachine context, StateFactory factory):base(context,factory){
        
    }


    public override void initSubState(){}
    public override void enter()
    {
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
            this.switchState(this.factory.createPunch1());
        }

        
    }
    public override void exit(){}
}

public class StateLassoWalk : State
{

    public StateLassoWalk(StateMachine context, StateFactory factory):base(context,factory){
        
    }

   
    public override void initSubState(){}

    public override void enter()
    {
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
            this.switchState(this.factory.createPunch1());
        }
    }
    public override void exit(){}

}

public class StateLassoPunch1 : State
{
    private float timerAttack = 0.5f;

    public StateLassoPunch1(StateMachine context, StateFactory factory):base(context,factory){
        this.timerAttack = 0.5f;
    }

    public override void enter()
    {
        Debug.Log("ENTER Punch");
        this.context.isPressedPunch1 = false;
        this.context.animator.Play(AnimationEnum.Punch1.ToString());

    }

    public override void exit(){}

    public override void initSubState(){}

    public override void update()
    {
        this.timerAttack -= Time.deltaTime;
        if(timerAttack < 0){
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

