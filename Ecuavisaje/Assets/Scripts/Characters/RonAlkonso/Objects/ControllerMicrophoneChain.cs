using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NetworkAnimator))]
public class ControllerMicrophoneChain : SkillObject
{
    [SerializeField] float forcePull = 3f;
    [SerializeField] float velocity = 10f;
    private BoxCollider boxCollider;
    private Animator animator;
    private bool collided = false;

    public override void OnStartServer()
    {

        base.OnStartServer();
        this.animator = this.objectMain.GetComponent<Animator>();
        this.boxCollider = this.GetComponent<BoxCollider>();
        StartCoroutine(this.animateForward());
        
        
        
    }

    public override void OnStartClient()
    {
        if(!hasAuthority) return;
        base.OnStartClient();
        this.gameObject.layer = LayerMask.NameToLayer("Ally");
    }

    void destroyMe(){
        // NetworkServer.Destroy(this.gameObject);
    }


    [ServerCallback]
    private void OnCollisionEnter(Collision other) {
        // Debug.Log("COLLISION my layer " + LayerMask.LayerToName(this.gameObject.layer));
        if(other.collider == this.ignoreCollider) return;

        if(other.gameObject.CompareTag("Character")){
            this.collided = true;
            // Debug.Log("DAMAGING " + other.gameObject.name);
            other.gameObject.GetComponent<Health>().damage(this.damage);
            
            StartCoroutine(this.animateBackward(other.gameObject));
        }
    }

    public IEnumerator animateForward(){

        float timeAnimationSeconds = this.animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        float timeCounter = 0f;
        Vector3 direction = new Vector3(1,0,0);

        while(timeCounter < timeAnimationSeconds/2){
            if(this.collided){
                break;
            }
            this.boxCollider.center += direction * this.velocity/2 * Time.deltaTime; // Move the object in the direction of scaling, so that the corner on ther side stays in place
            this.boxCollider.size += direction * this.velocity * Time.deltaTime; // Scale object in the specified direction


            timeCounter += Time.deltaTime;
            yield return null;           
        }

        if(!collided){
            this.destroyMe();
        }

    }

    public IEnumerator animateBackward(GameObject gameObject){
        this.animator.Play("Backward");

        Vector3 reposition = new Vector3(
            gameObject.transform.position.x-(5f*this.directionLooking),
            gameObject.transform.position.y,
            gameObject.transform.position.z
        );

        while(Vector3.Distance(gameObject.transform.position, reposition) > 0.2f){
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, reposition, forcePull * Time.deltaTime);
            yield return null;
        }
        this.destroyMe();
    }

}
