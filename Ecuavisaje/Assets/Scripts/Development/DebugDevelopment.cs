using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


enum AutoconnectionType
{
    none, host, server, client
}

public class DebugDevelopment : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Development")]
    [SerializeField] private bool is_debugging = true;
    [SerializeField] private bool exec_test = false;
    [SerializeField] private AutoconnectionType autoconnect = AutoconnectionType.none;

    private GameObject roomPlayer = null;
    private bool in_game = false;

    private GameObject networkRoomManager;
    void Start()
    {
        this.networkRoomManager = GameObject.FindGameObjectWithTag("NetworkRoomManager");
            

        if(this.is_debugging){
            this.networkRoomManager.SetActive(false);

            if(exec_test){
                this.test();
            }
            

            DontDestroyOnLoad(this.gameObject);
            
            switch (this.autoconnect)
            {
                case AutoconnectionType.host:
                    this.connectAsHost();
                    break;
                case AutoconnectionType.server:
                    this.connectAsServer();
                    break;
                case AutoconnectionType.client:
                    this.connectAsClient();
                    break;
                
                default:
                    break;
            }
            
        }
        else{
            this.networkRoomManager.SetActive(true);
        }    
    }

    private void test(){
        
    }

    private void Update() {
        if(!this.is_debugging) return;



        if(this.autoconnect == AutoconnectionType.none) return;

        this.networkRoomManager.SetActive(true);

        if(this.roomPlayer == null){
            this.roomPlayer = GameObject.FindGameObjectWithTag("RoomPlayer");
            return;
        }

        if(!in_game){
            this.roomPlayer.GetComponent<EcNetworkRoomPlayer>().CmdChangeReadyState(true);
            in_game = true;
            return;
        }
    }

    public void connectAsHost(){
        NetworkManager.singleton.StartHost();
    }
    public void connectAsServer(){
        NetworkManager.singleton.StartServer();
    }
    public void connectAsClient(){
        NetworkManager.singleton.StartClient();
    }

}
