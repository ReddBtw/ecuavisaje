using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ControllerBaston1 : SkillObject
{
    [SerializeField] float forceBegin = 2f;
    
    private BoxCollider boxCollider;
    public override void OnStartClient()
    {
        if(!hasAuthority) return;
        base.OnStartClient();
        this.gameObject.layer = LayerMask.NameToLayer("Ally");
    }
    
    void Start()
    {

        this.boxCollider = this.GetComponent<BoxCollider>();
        Destroy(this.gameObject, 3f);
    }

    [Command]
    public void cmdMoveMe(){
        this.transform.Translate(new Vector3(this.forceBegin * this.directionLooking * Time.deltaTime, 0, 0), Space.World);
        
        this.transform.RotateAround(
            boxCollider.bounds.center,
            new Vector3(1,0,0),
            100 * Time.deltaTime
        );
        
    }

    [ClientCallback]
    void Update()
    {
        if(!hasAuthority) return;
        this.cmdMoveMe();
    }

    [ServerCallback]
    private void OnCollisionEnter(Collision other) {
        if(other.collider == this.ignoreCollider) return;

        if(other.gameObject.tag == "Character"){
            Debug.Log("DAMAGING " + other.gameObject.name);
            other.gameObject.GetComponent<Health>().damage(this.damage);
        }
    }
}
