using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePunch : State
{
    protected float attackRange = 1f;
    protected float timerAttack = 0.5f;
    protected LayerMask layerPlayer;

    public StatePunch(CharacterStateMachine context, StateFactory factory):base(context,factory){
        this.timerAttack = 0.5f;
        this.layerPlayer = 1 << LayerMask.NameToLayer("Entity");
    }

    public override void enter()
    {
        if(State.VERBOSE_LEVEL > 0)
            Debug.Log("ENTER Punch");
        this.context.isPressedPunch1 = false;
        this.context.cmdPlaySound(this.context.getCharacterEnum(), CharacterAudioEnum.Punch);
        this.context.animator.Play(AnimationEnum.Punch1.ToString());

        this.context.cmdAttackPunch(this.attackRange, this.layerPlayer);

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
