using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class DebugDevelopment : MonoBehaviour
{
    [SerializeField] bool autohost = true;
    // Start is called before the first frame update
    void Start()
    {
        if(this.autohost){
        NetworkManager.singleton.StartHost();
        Debug.Log("Start host");
        }
    }

}
