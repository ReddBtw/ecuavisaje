using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Special1 : SkillObject
{
    [SerializeField] float forceBegin = 2f;

    
    void Start()
    {
        Destroy(this.gameObject, 3f);
    }

    [Command]
    public void cmdMoveMe(){
        this.transform.Translate(new Vector3(this.forceBegin * Time.deltaTime, 0, 0), Space.World);
        this.transform.RotateAround(
            this.GetComponent<BoxCollider>().bounds.center,
            new Vector3(1,0,0),
            500 * Time.deltaTime
        
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
