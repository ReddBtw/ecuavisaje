using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DebugDevelopment : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool is_debugging = true;
    [SerializeField] private bool autoconnect = true;
    [SerializeField] private GameObject player1Test = null;
    [SerializeField] private GameObject player2Test = null;

    void Start()
    {
        if(this.is_debugging){
            if(this.autoconnect){
                this.connectAsHost();
            }
            else{
                this.player1Test.gameObject.SetActive(true);
                this.player1Test.GetComponent<Player>().opponent = this.player2Test;
            }
            
        }
        
        
    }
    public void connectAsHost(){
        NetworkManager.singleton.StartHost();
    }

}
