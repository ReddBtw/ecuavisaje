using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicFeet : MonoBehaviour
{
    public SurfaceDisplacement surfaceDisplacement;
    // Start is called before the first frame update
    void Start()
    {
        surfaceDisplacement = GetComponentInParent<SurfaceDisplacement>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerStay()
    {
        //Si el personaje esta tocando una superficie puede saltar
        surfaceDisplacement.puedeSaltar = true;
    }
    void OnTriggerExit(Collider other)
    {
        //Si el personaje sale de la superficie no puede saltar
        surfaceDisplacement.puedeSaltar = false;
    }
}
