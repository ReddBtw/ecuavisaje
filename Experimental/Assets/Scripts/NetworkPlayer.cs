using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] GameObject prefab1;
    [SerializeField] GameObject prefab2;
    [SerializeField] TMPro.TMP_Text displayBarName;

    // sync server value to client
    [SyncVar(hook = nameof(handleUpdateDisplayName))]
    [SerializeField]
    private string display_name = "Missing name";

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("LOCAL:OnStartServer |" + netIdentity.connectionToClient.connectionId + "|");
    }
    public override void OnStartClient()
    {
        if(!isLocalPlayer) return;
        base.OnStartClient();
        Debug.Log("LOCAL:OnStartClient NetworkClient.connection.connectionId |" + NetworkClient.connection.connectionId + "|");
        
        Debug.Log("LOCAL:OnStartClient connectionToServer |" + this.connectionToServer + "|");
        Debug.Log("LOCAL:OnStartClient STARTED TRULY isLocalPlayer:" + isLocalPlayer);
        //Debug.Log("CREATING OWNED");
        this.cmdCreate();
    }

    [ClientCallback]
    private void Update() {
        if(!hasAuthority) return;
        if(this.connectionToServer != null){
            // Debug.Log("LOCAL: connectionToServer |" + this.connectionToServer.connectionId + "|");
        }

        if(Input.GetKeyDown(KeyCode.A)){
            this.cmdRequestRPC();
        }
    }

    [Command]
    public void cmdRequestRPC(){
        this.rpcTest();
    }

    [ClientRpc]
    public void rpcTest(){
        Debug.Log("NetworkPlayer.Client rpc called: isLocalPlayer-" + isLocalPlayer + " hasAuthority-" + hasAuthority + ". NetworkServer.active-" + NetworkServer.active);
    }

    [Command]
    public void cmdCreateInself(){
        GameObject obj = Instantiate(this.prefab2, this.transform);
        NetworkServer.Spawn(obj, connectionToClient);
    }


    [Command]
    public void cmdCreate(){
        GameObject obj = Instantiate(this.prefab1, this.transform.position, this.transform.rotation);
        NetworkServer.Spawn(obj, connectionToClient);
    }

    [Server]
    public void setDisplayName(string new_display_name){
        display_name = new_display_name;
    }


    private void handleUpdateDisplayName(string oldName, string newName)
    {
        display_name = newName;
        //this.displayBarName.text = newName;
    }



}
