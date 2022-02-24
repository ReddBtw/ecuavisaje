using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
﻿using System;



public class NetworkPlayer : NetworkBehaviour
{

    [SyncVar(hook = nameof(handleUpdateId))]
    public int id = -1;
    [SyncVar(hook = nameof(handleUpdateCharacterEnum))]
    public CharacterEnum characterEnum = CharacterEnum.None;

    public GameObject opponent {get; set;}


    [SerializeField] private Character[] characters = new Character[0];


    #region Server

    public override void OnStartServer()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    [Server]
    public void setPlayer(int index){
        id = index;
        
    }

    #endregion


    #region Commands
    
    [Command]
    public void cmdSetCharacter(CharacterEnum characterEnum){
        this.characterEnum = characterEnum;

        Character characterSelected = null;
        foreach (Character character in this.characters)
        {
            if(character.characterEnum == characterEnum){
                characterSelected = character;
                break;
            }
        }

        Debug.Assert(characterSelected != null);

        GameObject characterInstance =
            Instantiate(characterSelected.characterPrefab, connectionToClient.identity.transform.position, connectionToClient.identity.transform.rotation);

        NetworkServer.Spawn(characterInstance, connectionToClient);
    }

    #endregion

    #region Client
    public override void OnStartClient()
    {
        if(!hasAuthority) { return; }

        

        this.cmdSetCharacter(CharacterEnum.Lasso);

        if (NetworkServer.active) { return; }

        DontDestroyOnLoad(this.gameObject);
        ((EcNetworkRoomManager)NetworkManager.singleton).players.Add(this);
        
    }


    public override void OnStopClient()
    {
    
        if (NetworkServer.active) { return; }
        ((EcNetworkRoomManager)NetworkManager.singleton).players.Remove(this);

    }

    [Client]
    private void handleUpdateId(int oldValue, int newValue)
    {
        this.id = newValue;
    }

    [Client]
    private void handleUpdateCharacterEnum(CharacterEnum oldValue, CharacterEnum newValue)
    {
        this.characterEnum = newValue;

    } 

    #endregion
    
}
