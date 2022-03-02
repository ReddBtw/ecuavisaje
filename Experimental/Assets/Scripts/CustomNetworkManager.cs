using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    public static int COUNTER = 0;

    public override void OnClientConnect(NetworkConnection conn){
       base.OnClientConnect(conn);
 
       Debug.Log("I connected to server");
 
 
   }

   public override void OnServerAddPlayer(NetworkConnection conn){
       base.OnServerAddPlayer(conn);

       CustomNetworkManager.COUNTER += 1;
 
       // reference to NetworkIdentity
       NetworkPlayer player = conn.identity.GetComponent<NetworkPlayer>();
 
       player.setDisplayName($"Jugador {CustomNetworkManager.COUNTER}");
 
       Debug.Log($"Add player: {base.numPlayers}");
 
 
 
   }


}
