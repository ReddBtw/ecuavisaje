using UnityEngine;

public class StateUltimateRonAlkonso : State
{
    public float timerDuration = 1f;
    protected LayerMask layerPlayer;

    public StateUltimateRonAlkonso(CharacterStateMachine context, StateFactory factory):base(context,factory){
        this.layerPlayer = 1 << LayerMask.NameToLayer("Entity");
    }

    public override void enter()
    {
        if(State.VERBOSE_LEVEL > 0)
            Debug.Log("ENTER SPECIAL1");
        this.context.isActivatedUltimate = false;
        this.context.characterCommandGiver.cmdPlaySound(this.context.getCharacterEnum(), AnimationEnum.Ultimate);
        this.context.animator.Play(AnimationEnum.Ultimate.ToString());
        // this.context.characterCommandGiver.cmdInvokeSpecial1(this.context.getCharacterEnum());

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