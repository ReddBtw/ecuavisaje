using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

public class OwnedObject : NetworkBehaviour
{
    GameObject opponent;
    CinemachineBrain cameraBrain;
    CinemachineVirtualCamera camera1;
    CinemachineVirtualCamera camera2;

    public override void OnStartServer()
    {
        base.OnStartServer();
        //Debug.Log("OWNED:OnStartServer |" + netIdentity.connectionToClient.connectionId + "|");
        NetworkPlayer[] players = GameObject.FindObjectsOfType<NetworkPlayer>();
        foreach (var item in players)
        {
            Debug.Log(item);
        }
        
        cameraBrain = GameObject.FindObjectOfType<CinemachineBrain>();
        CinemachineVirtualCamera[] cameras = GameObject.FindObjectsOfType<CinemachineVirtualCamera>();

        foreach (var item in cameras)
        {
            if(item.name.Contains("1")){
                camera1 = item;
            }
            else{
                camera2 = item;
            }
        }

        StartCoroutine(startAnimation());
    }

    public override void OnStartClient()
    {
        //Debug.Log("hasAuthority: " + hasAuthority);
        if(!hasAuthority) return;
        base.OnStartClient();
        //Debug.Log("OWNED:OnStartClient NetworkClient.connection.connectionId |" + NetworkClient.connection.connectionId + "|");
        //Debug.Log("OWNED:OnStartClient NetworkClient.connection.identity |" + NetworkClient.connection.identity + "|");
 

        NetworkPlayer[] players = GameObject.FindObjectsOfType<NetworkPlayer>();
        foreach (NetworkPlayer item in players)
        {
            //Debug.Log("OWNED:OnStartClient:otherplayers:p1" + item.connectionToServer);
            if(item.connectionToServer != null){
            //    Debug.Log("OWNED:OnStartClient:otherplayers:extra" + (item.connectionToServer.identity == NetworkClient.connection.identity));
            }
            
        }

        // Debug.Log("OWNED:OnStartClient |" + NetworkClient.connection.connectionId + "|");
        //Debug.Log("STARTED OWNED isLocalPlayer:" + isLocalPlayer);

        
    }



    private void Update() {

        
        
        if(!hasAuthority) return;
        
        if(this.connectionToServer != null){
            // Debug.Log("LOCAL: connectionToServer |" + this.connectionToServer.connectionId + "|");
        }

        if(Input.GetKeyDown(KeyCode.S)){
            this.cmdRequestRPC();
        }
        if(Input.GetKeyDown(KeyCode.D)){
            this.cmdTP();
        }
        if(Input.GetKeyDown(KeyCode.W)){
            this.cmdAnimation();
        }

        if(opponent == null){
            findOpponent();
        }
    }
    public void findOpponent(){
        OwnedObject[] owneds = GameObject.FindObjectsOfType<OwnedObject>();
        foreach (OwnedObject item in owneds)
        {
            if(item == this){
                Debug.Log("Found this!");
            }
            else{
                Debug.Log("OTHER");
                this.opponent = item.gameObject;
            }
        }
    }

    [Command]
    public void cmdAnimation(){
        if(this.opponent == null){
            findOpponent();
        }
        StartCoroutine(startAnimation());
        
    }

    [Server]
    public IEnumerator startAnimation(){
        camera1.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false);
        this.rpcTest("BEGIN ANIMATION");
        Vector3 position1 = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z-5f);
        
        while (Vector3.Distance(this.camera1.transform.position, position1) > 0.1f)
        {
            this.camera1.transform.position = Vector3.Lerp(camera1.transform.position, position1, Time.deltaTime);
            yield return null;
        }
        this.rpcTest("WAITING 1");
        yield return new WaitForSeconds(1);
        camera1.gameObject.SetActive(false);
        camera2.gameObject.SetActive(true);
        this.rpcTest("STOP ANIMATION");
    }

    [Command]
    public void cmdTP(){
        this.transform.position = new Vector3(1,3,4);
    }

     [Command]
    public void cmdRequestRPC(){
        this.rpcTest("");
    }

    [ClientRpc]
    public void rpcTest(string message){
        if(message == ""){
            Debug.Log("OwnedObject.Client rpc called: isLocalPlayer-" + isLocalPlayer + " hasAuthority-" + hasAuthority + ". NetworkServer.active-" + NetworkServer.active);
        }else{
            Debug.Log(message);
        }
        
    }
}
