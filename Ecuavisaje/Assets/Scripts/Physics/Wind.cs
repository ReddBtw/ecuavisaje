using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]
public class Wind : SkillObject
{
    [SerializeField] float wind_force = 10f;
    [SerializeField] Vector2 offset_y;
    
    private Rigidbody rb;
    void Start()
    {
        this.rb = this.GetComponent<Rigidbody>();
        this.directionLooking = 1;

        Vector3 vectorForce = new Vector3(
            this.directionLooking * this.wind_force,
            Random.Range(offset_y.x, offset_y.y),
            0
        );

        Vector3 vectorTorque = new Vector3(
            Random.Range(-1, 1),
            Random.Range(-1, 1),
            Random.Range(-1, 1)
        );

        this.rb.AddForce(vectorForce, ForceMode.VelocityChange);

        this.rb.AddTorque(vectorTorque, ForceMode.Impulse);

        Invoke("destroyThis", 3f);

    }

    void destroyThis(){
        NetworkServer.Destroy(this.gameObject);
    }



}
