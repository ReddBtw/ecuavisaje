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
    public void cmdInvokeSpecial1(CharacterEnum characterEnum){

        foreach (Character character in this.characters)
        {   
            if(character.characterEnum == characterEnum){
                if(character.special1.skillType == SkillType.throw_object){
                    
                    Vector3 vectorPosition = new Vector3(this.transform.position.x+2,this.transform.position.y+4,this.transform.position.z);
                    Quaternion rotation = Quaternion.Euler(
                        character.special1.gameObjectPrefab.transform.rotation.x, 
                        90, // this.transform.rotation.y not working todo: why? 
                        -90

                    );



                    GameObject instantiateSpecial1 = Instantiate(character.special1.gameObjectPrefab, vectorPosition, rotation);
                    instantiateSpecial1.GetComponent<Special1>().ignoreCollider = this.GetComponent<CapsuleCollider>();
                    
                    NetworkServer.Spawn(instantiateSpecial1, this.connectionToClient);
                }
            }
            
        }
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
