using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Animator))]
public class ControllerJojoStand : SkillObject
{
    
    [SerializeField] GameObject pointHitPunch;

    
    private BoxCollider boxCollider;
    private Animator animator;
    

    public override void OnStartServer()
    {

        base.OnStartServer();
        this.animator = this.GetComponent<Animator>();
        this.boxCollider = this.GetComponent<BoxCollider>();
        if(hasAuthority){
            this.gameObject.layer = LayerMask.NameToLayer("Ally");
        }
        
        StartCoroutine(this.animate());
        
        
        
    }

    public override void OnStartClient()
    {
        if(!hasAuthority) return;
        base.OnStartClient();
        this.gameObject.layer = LayerMask.NameToLayer("Ally");
    }

    void destroyMe(){
        NetworkServer.Destroy(this.gameObject);
    }

    public IEnumerator animate(){

        Debug.Log("My layer " + LayerMask.LayerToName(this.gameObject.layer));
        float timeAnimationSeconds = this.animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        /*
        while(){
            
            yield return null;
        }
        */
        yield return new WaitForSeconds(timeAnimationSeconds/2f); // divided by animation velocity reproduction

        int layerMask = 1 << LayerMask.NameToLayer("Enemy");
        Collider[] hits = Physics.OverlapSphere(this.pointHitPunch.transform.position, this.attackRange, layerMask);

        foreach (Collider hit in hits)
        {
            if(hit == this.ignoreCollider) continue;
            Health h = hit.gameObject.GetComponent<Health>();
            h.damage(this.damage);
            
        }
        this.destroyMe();

    }

}
