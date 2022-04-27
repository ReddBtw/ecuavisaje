using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinematic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(demo());
    }

    IEnumerator demo(){
        Debug.Log("Hey");
        yield return new WaitForSeconds(1);
        Debug.Log("hola");
    }
    // Update is called once per frame
    void Update()
    {

    }
}
