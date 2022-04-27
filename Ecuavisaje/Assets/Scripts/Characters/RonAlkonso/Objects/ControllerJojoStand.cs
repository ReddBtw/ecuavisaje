using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(NetworkAnimator))]
public class ControllerJojoStand : SkillObject
{
    
    [SerializeField] GameObject pointHitPunch;

    
    private Animator animator;
    private Vector3 positionInitialOffset;
    

    public override void OnStartServer()
    {

        base.OnStartServer();
        this.animator = this.objectMain.GetComponent<Animator>();

        if(hasAuthority){
            this.objectMain.layer = LayerMask.NameToLayer("Ally");
        }
        
        this.positionInitialOffset = this.transform.position - this.parentServer.transform.position;

        StartCoroutine(this.animate());
        
        
        
    }

    public override void OnStartClient()
    {
        if(!hasAuthority) return;
        base.OnStartClient();
        this.gameObject.layer = LayerMask.NameToLayer("Ally");
    }


    [ServerCallback]
    private void Update() {
        this.transform.position = this.parentServer.transform.position + this.positionInitialOffset;
    }

    void destroyMe(){
        NetworkServer.Destroy(this.gameObject);
    }

    public IEnumerator animate(){

        // Debug.Log("My layer " + LayerMask.LayerToName(this.gameObject.layer));
        // float timeAnimationSeconds = this.animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        /*
        while(){
            
            yield return null;
        }
        */
        yield return new WaitForSeconds(0.3f); // divided by animation velocity reproduction

        // int layerMask = 1 << LayerMask.NameToLayer("Enemy");
        Collider[] hits = Physics.OverlapSphere(this.pointHitPunch.transform.position, this.attackRange);

        // Debug.Log("HITS " + hits.Length);

        foreach (Collider hit in hits)
        {
            if(hit == this.ignoreCollider) continue;
            if(hit.gameObject.CompareTag("Character")){
                Health h = hit.gameObject.GetComponent<Health>();
                h.damage(this.damage);
            }
            
        }
        yield return new WaitForSeconds(1f); // divided by animation velocity reproduction
        this.destroyMe();

    }

}
