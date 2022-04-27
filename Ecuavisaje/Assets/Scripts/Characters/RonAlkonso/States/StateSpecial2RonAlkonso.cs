using UnityEngine;

public class StateSpecial2RonAlkonso : State
{
    public float timerDuration = 1f;
    protected LayerMask layerPlayer;

    public StateSpecial2RonAlkonso(CharacterStateMachine context, StateFactory factory):base(context,factory){
        this.layerPlayer = 1 << LayerMask.NameToLayer("Entity");
    }

    public override void enter()
    {
        if(State.VERBOSE_LEVEL > 0)
            Debug.Log("ENTER SPECIAL2");
        this.context.isActivatedSpecial2 = false;
        this.context.isAttacking = true; // todo: remove later
        this.context.characterCommandGiver.cmdPlaySound(this.context.getCharacterEnum(), AnimationEnum.Special2);
        this.context.animator.Play(AnimationEnum.Special2.ToString());
        this.context.characterCommandGiver.cmdInvokeSpecial(this.context.getCharacterEnum(), 2);

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