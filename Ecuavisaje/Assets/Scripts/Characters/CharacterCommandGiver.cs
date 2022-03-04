using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterStateMachine))]
public class CharacterCommandGiver : NetworkBehaviour
{

    [SerializeField] private GameObject pointHitPunch;
    private Rigidbody rb;
    public  AudioSource audioSource {get;set;} 

    private List<Character> characters;
    private CharacterStateMachine characterStateMachine;
    private void Start() {
        this.characterStateMachine = this.GetComponent<CharacterStateMachine>();
        this.characters = UtilsResources.getScriptableObjectsCharacters();
        this.rb = this.GetComponent<Rigidbody>();
        this.audioSource = this.GetComponent<AudioSource>();
    }

    #region Command
    
    [Command]
    public void cmdMoveX(float x){
        this.transform.Translate(x, 0, 0, Space.World);
    }


    [Command]
    public void cmdJump(float force){
        this.rb.AddForce(new Vector3(0,force,0), ForceMode.Force);
    }

    [Command]
    public void cmdLookAt(float x){
        
        this.transform.LookAt(new Vector3(
            x,
            this.transform.position.y,
            this.transform.position.z
        ));
    }

    [Command]
    public void cmdPlaySound(CharacterEnum characterEnum, AnimationEnum animationEnum){
        this.rpcPlayAudio(characterEnum, animationEnum);
    }

    [Command]
    public void cmdAttackPunch1(float attackRange, int layer){
        Collider[] hits = Physics.OverlapSphere(this.pointHitPunch.transform.position, attackRange, layer);

        foreach (Collider hit in hits)
        {
            if(hit == this.gameObject.GetComponent<CapsuleCollider>()) continue;
            Health h = hit.gameObject.GetComponent<Health>();
            h.damage(this.characterStateMachine.characterSelected.punch1.damage);           
            
        }
    }



    [Command]
    public void cmdInvokeSpecial(CharacterEnum characterEnum, int specialId){

        int directionLooking = (this.transform.rotation.eulerAngles.y > 180)? -1: 1;
        // to left=-1, to right=1

        foreach (Character character in this.characters)
        {   
            if(character.characterEnum == characterEnum){

                Skill special = character.special1;
                if(specialId == 2){
                    special = character.special2;
                }

                Vector3 vectorPosition = this.transform.position;

                if(special.skillType == SkillType.throw_object){
                    vectorPosition = new Vector3(this.transform.position.x+(3*directionLooking),this.transform.position.y+4,this.transform.position.z);
                    
                }
                else if(special.skillType == SkillType.invocable){
                    vectorPosition = new Vector3(this.transform.position.x+(special.startPositionOffset.x*directionLooking),this.transform.position.y+special.startPositionOffset.y,this.transform.position.z); 

                }

                Quaternion rotation = Quaternion.Euler(
                    special.gameObjectPrefab.transform.rotation.eulerAngles.x,
                    special.gameObjectPrefab.transform.rotation.eulerAngles.y + ((directionLooking<0)? -90:90),
                    special.gameObjectPrefab.transform.rotation.eulerAngles.z

                );

                GameObject instantiateSpecial = Instantiate(special.gameObjectPrefab, vectorPosition, rotation);
                instantiateSpecial.GetComponent<SkillObject>().ignoreCollider = this.GetComponent<CapsuleCollider>();
                instantiateSpecial.GetComponent<SkillObject>().damage = special.damage;
                instantiateSpecial.GetComponent<SkillObject>().directionLooking = directionLooking;
                instantiateSpecial.GetComponent<SkillObject>().attackRange = special.attackRange;
                NetworkServer.Spawn(instantiateSpecial, this.connectionToClient);
            }
            
        }
    }

    [Command]
    public void cmdUltimate(CharacterEnum characterEnum){
        StateUltimateRonAlkonso.callbackUltimate(this.characterStateMachine);
    }

    

    #endregion

    #region RPC

    [ClientRpc]
    public void rpcPlayAudio(CharacterEnum characterEnum, AnimationEnum animationEnum){
        // todo: optimice with hashing?
        // todo: repair play audio

        foreach (Character character in this.characters)
        {
            if(character.characterEnum == characterEnum){

                switch (animationEnum)
                {
                    case AnimationEnum.Punch1:
                        SystemAudio.playAtPosition(character.punch1.audio, this.transform.position);
                        break;
                    case AnimationEnum.Special1:
                        SystemAudio.playAtPosition(character.special1.audio, this.transform.position);
                        break;
                    default:
                        break;
                }
                
            }            
        }
        
        
    }

    [ClientRpc]
    public void rpcTest(string message){
        if(message == ""){
            Debug.Log("CharacterCommandGiver.rpcTest: isLocalPlayer-" + isLocalPlayer + " hasAuthority-" + hasAuthority + ". NetworkServer.active-" + NetworkServer.active);
        }else{
            Debug.Log(message);
        }
        
    }
        
    #endregion
    
}
