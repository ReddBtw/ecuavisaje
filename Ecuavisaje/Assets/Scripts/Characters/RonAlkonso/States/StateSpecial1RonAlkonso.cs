using UnityEngine;

public class StateSpecial1RonAlkonso : State
{
    public float timerDuration = 1f;
    protected LayerMask layerPlayer;

    public StateSpecial1RonAlkonso(CharacterStateMachine context, StateFactory factory):base(context,factory){
        this.layerPlayer = 1 << LayerMask.NameToLayer("Entity");
    }

    public override void enter()
    {
        if(State.VERBOSE_LEVEL > 0)
            Debug.Log("ENTER SPECIAL1");
        this.context.isActivatedSpecial1 = false;
        this.context.isAttacking = true; // todo: remove duplicate
        this.context.characterCommandGiver.cmdPlaySound(this.context.getCharacterEnum(), AnimationEnum.Special1);
        this.context.animator.Play(AnimationEnum.Special1.ToString());
        this.context.characterCommandGiver.cmdInvokeSpecial(this.context.getCharacterEnum(), 1);

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