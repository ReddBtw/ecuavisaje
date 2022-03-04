using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mirror;

public class StateUltimateRonAlkonso : State
{
    public float timerDuration = 5f;
    protected LayerMask layerPlayer;

    public StateUltimateRonAlkonso(CharacterStateMachine context, StateFactory factory):base(context,factory){
        this.layerPlayer = 1 << LayerMask.NameToLayer("Entity");
    }

    public override void enter()
    {
        if(State.VERBOSE_LEVEL > 0)
            Debug.Log("ENTER SPECIAL1");
        this.context.isActivatedUltimate = false;
        this.context.isAttacking = true; // todo: remove duplicate
        this.context.characterCommandGiver.cmdPlaySound(this.context.getCharacterEnum(), AnimationEnum.Ultimate);
        this.context.characterCommandGiver.cmdUltimate(this.context.getCharacterEnum());
    }


    public static void callbackUltimate(CharacterStateMachine context){
        // called on server
        context.StartCoroutine(ultimate(context));
    }

    public static IEnumerator ultimate(CharacterStateMachine context){
        // called on server
        yield return null;
        context.cutSceneController.cmdAnimation();
        yield return new WaitForSeconds(1f);
        context.animator.Play(AnimationEnum.Ultimate.ToString());
        yield return new WaitForSeconds(1f);
        context.animator.Play(AnimationEnum.Punch1.ToString());

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
            Debug.Log("SWITCHING FROM ULTIMATE TO " + this.context.isMoving);
            if(this.context.isMoving){
                this.switchState(this.factory.createWalk());
            }
            else{
                this.switchState(this.factory.createIdle());
            }
        }
        
        
    }
}