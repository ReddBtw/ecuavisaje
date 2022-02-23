using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class EcuavisajeNetworkManager : NetworkManager
{
    public List<Player> players { get; } = new List<Player>();
    
    private bool isGameInProgress = false;

    #region Server
    
    public override void OnServerConnect(NetworkConnection conn)
    {
        if (!isGameInProgress) { return; }
        // kick hacker
        conn.Disconnect();
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        Player player = conn.identity.GetComponent<Player>();

        this.players.Remove(player);

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        this.players.Clear();
        this.isGameInProgress = false;
    }

    public void startGame()
    {
        if (this.players.Count < 2) { return; }

        this.isGameInProgress = true;

        // todo: program the lobby and change scene with this
        // ServerChangeScene("Scene_Map_01");
    }

    public override void OnServerAddPlayer(NetworkConnection conn){
        base.OnServerAddPlayer(conn);
        
        Player player = conn.identity.GetComponent<Player>();
        players.Add(player);
        
        if(players.Count == 1){
            player.setId(0);
        }
        else{
            player.setId(1);
        };
        
        // todo: put menu button to start game
        if(players.Count == 2){

            players[0].opponent = GameObject.Find("T"); // players[1].gameObject;
            players[1].opponent = GameObject.Find("T"); // players[0].gameObject;
            

            foreach (Player p in players)
            {
                int id = 1;
                
                Character characterSelected = null;
                foreach (Character character in p.GetCharacters())
                {
                    if(character.getId() == id){
                        characterSelected = character;
                        break;
                    }
                }

                if(characterSelected == null) return;


                GameObject characterInstance =
                    Instantiate(characterSelected.getModel(), p.transform.position, p.transform.rotation);
                
                // only is set in server
                // this breaks the cliend side!!!!
                // characterInstance.transform.parent = p.connectionToClient.identity.transform;
                
                NetworkServer.Spawn(characterInstance, p.connectionToClient);
                
            }



        }
 
    }

    #endregion

}
