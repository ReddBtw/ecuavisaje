using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Spawner : NetworkBehaviour
{
    [SerializeField] GameObject[] objects = new GameObject[0];
    private int count_spawned = 0;
    [SerializeField] private int total_spawns = 1;
    [SerializeField] private float time_wait = 1f;

    void Start()
    {
        StartCoroutine(spawnObjects());
    }

    IEnumerator spawnObjects(){
        int num_objects = this.objects.Length;

        if(num_objects == 0){
            yield return null;
        }

        while(count_spawned < total_spawns){
            int index = Random.Range(0, num_objects-1);
            GameObject objectInstance = Instantiate(this.objects[index], this.transform.position, this.transform.rotation);
            NetworkServer.Spawn(objectInstance);
            yield return new WaitForSeconds(this.time_wait);
            count_spawned += 1;
        }

        
    }
}
