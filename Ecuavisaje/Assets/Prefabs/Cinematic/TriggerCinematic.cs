using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCinematic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.activate();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Character")){
            this.activate();
        }
    }

    public virtual void activate(){

    }
}
