using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Cinemachine;

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
        GameObject sender = context.gameObject;

        CutSceneController cutSceneController = context.cutSceneController;


        // context.characterCommandGiver.rpcTest("SCENE1");
        cutSceneController.setCamera(CameraVirtualName.camera1);

        // this.rpcTest("BEGIN ANIMATION");
        Vector3 position1 = new Vector3(
            sender.transform.position.x, 
            sender.transform.position.y+7, 
            sender.transform.position.z-10f
        );

        float velocityZoom = 3f;
        
        context.animator.Play(AnimationEnum.Ultimate.ToString());
        while (Vector3.Distance(cutSceneController.camera1.transform.position, position1) > 0.5f)
        {
            cutSceneController.camera1.transform.position = Vector3.Lerp(cutSceneController.camera1.transform.position, position1, velocityZoom * Time.deltaTime);
            yield return null;
        }
        // context.characterCommandGiver.rpcTest("SCENE2");
        yield return new WaitForSeconds(0.5f);
        cutSceneController.setCamera(CameraVirtualName.camera2);
        context.animator.Play(AnimationEnum.Punch1.ToString());
        // context.characterCommandGiver.rpcTest("SCENE_ULTIMATE");
        yield return new WaitForSeconds(0.5f);
        cutSceneController.setCamera(CameraVirtualName.main);
        context.animator.Play(AnimationEnum.Kick1.ToString());
        cutSceneController.camera1.transform.position = cutSceneController.main.transform.position;

        int directionLooking = (context.transform.rotation.eulerAngles.y > 180)? -1: 1;
        // to left=-1, to right=1


        // context.characterCommandGiver.rpcTest("CL: " + context.characters.Count + ", context.getCharacterEnum(): " + context.getCharacterEnum());


        foreach (Character character in context.characters)
        {   
            if(character.characterEnum == context.getCharacterEnum()){

                Skill ultimate = character.ultimate;

                Vector3 vectorPosition = context.transform.position;

                if(ultimate.skillType == SkillType.throw_object){
                    vectorPosition = new Vector3(context.transform.position.x+(3*directionLooking),context.transform.position.y+4,context.transform.position.z);
                    
                }
                else if(ultimate.skillType == SkillType.invocable){
                    vectorPosition = new Vector3(context.transform.position.x+(ultimate.startPositionOffset.x*directionLooking),context.transform.position.y+ultimate.startPositionOffset.y,context.transform.position.z); 

                }

                Quaternion rotation = Quaternion.Euler(
                    ultimate.gameObjectPrefab.transform.rotation.eulerAngles.x,
                    ultimate.gameObjectPrefab.transform.rotation.eulerAngles.y + ((directionLooking<0)? -90:90),
                    ultimate.gameObjectPrefab.transform.rotation.eulerAngles.z

                );

                cutSceneController.rpcShake(CameraVirtualName.main, 5);
                context.characterCommandGiver.instantiateSkillObject(ultimate, vectorPosition, rotation, directionLooking);
                yield return new WaitForSeconds(3f);
                cutSceneController.rpcShake(CameraVirtualName.main, 0);
                break;
            }
            
        }
        // context.characterCommandGiver.rpcTest("SCENE_END");

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
            // Debug.Log("SWITCHING FROM ULTIMATE TO " + this.context.isMoving);
            if(this.context.isMoving){
                this.switchState(this.factory.createWalk());
            }
            else{
                this.switchState(this.factory.createIdle());
            }
        }
        
        
    }
}