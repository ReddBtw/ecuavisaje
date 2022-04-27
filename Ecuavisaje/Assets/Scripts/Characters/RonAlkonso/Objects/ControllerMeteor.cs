using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class ControllerMeteor : SkillObject
{
    private bool collided = false;
    [SerializeField] private float impulse = 2f;
    [SerializeField] public GameObject particleSystemExplosion;


    private GameObject gameObjectExplosion;

    public override void OnStartServer()
    {

        base.OnStartServer();
        Invoke("destroyMe", 5f);
        this.GetComponent<Rigidbody>().AddForce(this.impulse * Time.deltaTime * this.directionLooking * new Vector3(1,0,0), ForceMode.Impulse);
        
        
    }

    public override void OnStartClient()
    {
        if(!hasAuthority) return;
        base.OnStartClient();
        this.gameObject.layer = LayerMask.NameToLayer("Ally");
    }


    void destroyMe(){
        if(!collided){
            NetworkServer.Destroy(this.gameObject);
        }
    }

    public IEnumerator destroyMeteor(){
        yield return new WaitForSeconds(2f);
        NetworkServer.Destroy(gameObjectExplosion);
    }

    [ServerCallback]
    private void OnCollisionEnter(Collision other) {
        // Debug.Log("COLLISION my layer " + LayerMask.LayerToName(this.gameObject.layer));
        if(other.collider == this.ignoreCollider) return;

        if(gameObjectExplosion == null){
            gameObjectExplosion = Instantiate(this.particleSystemExplosion, this.transform.position, this.transform.rotation);
            NetworkServer.Spawn(gameObjectExplosion);
            StartCoroutine(destroyMeteor());
            Debug.Log("Spawn explosion");
        }
        

        if(other.gameObject.CompareTag("Character")){
            this.collided = true;
            // Debug.Log("DAMAGING " + other.gameObject.name);
            other.gameObject.GetComponent<Health>().damage(this.damage);
            NetworkServer.Destroy(this.gameObject);
        }
    }

}
