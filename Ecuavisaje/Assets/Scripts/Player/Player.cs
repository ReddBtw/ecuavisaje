using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
﻿using System;

public class Player : NetworkBehaviour
{

    [SyncVar(hook = nameof(handleUpdateId))]
    public int id = -1;


    public GameObject opponent {get; set;}

    public GameObject character {get; set;} = null;

    [SerializeField] private Character[] characters = new Character[0];
    [SerializeField] private Transform[] spawnPointsTransforms = new Transform[0];

    public Character[] GetCharacters(){
        return this.characters;
    }

    #region Server

    public override void OnStartServer()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    [Server]
    public void setId(int index){
        // 0=left, 1=right
        // todo: check if spawn point is better
        // Debug.Assert(index >= 0 && index < spawnPointsTransforms.Length);
        id = index;
        // spawnPosition = spawnPointsTransforms[index].position;
    }



    #endregion

    #region Client


    [Client]
    public override void OnStartClient()
    {

        if (NetworkServer.active) { return; }
        // if (!hasAuthority) { return; }

        DontDestroyOnLoad(this.gameObject);
        ((EcuavisajeNetworkManager)NetworkManager.singleton).players.Add(this);


        
        
    }


    public override void OnStopClient()
    {
        

        if (!isClientOnly) { return; }

        ((EcuavisajeNetworkManager)NetworkManager.singleton).players.Remove(this);

        if (!hasAuthority) { return; }

    }

    [Client]
    private void handleUpdateId(int oldValue, int newValue)
    {
        this.id = newValue;
    }


    

    

    #endregion
    
    

    


}
