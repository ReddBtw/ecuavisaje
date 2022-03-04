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


    private List<Character> characters;

    // Linking Network Player with Main Character instance script
    private CharacterStateMachine characterCurrent; 


    public CharacterStateMachine getCharacterStateMachine(){
        return this.characterCurrent;
    }

    private void Awake() {
        this.characters = UtilsResources.getScriptableObjectsCharacters();
    }

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
        Debug.Log("CREATING: " + characterEnum);
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


        // call the hook in player to say: I instantiated your character
        this.characterEnum = characterEnum;
        this.characterCurrent = characterInstance.GetComponent<CharacterStateMachine>();
    }

    #endregion

    #region Client
    public override void OnStartClient()
    {
        if(!hasAuthority) { return; }

        try{
        	if(((EcNetworkRoomManager)NetworkManager.singleton).players.Count > 0){
                this.cmdSetCharacter(CharacterEnum.RonAlkonso);
            }
            else{
                this.cmdSetCharacter(CharacterEnum.Lasso);

            }
        
        }
        catch (Exception)
        {
            // Debug.Log("Selecting default");
        	this.cmdSetCharacter(CharacterEnum.RonAlkonso);
        }
        

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

        CharacterStateMachine[] gameObjectCharacters = GameObject.FindObjectsOfType<CharacterStateMachine>();
        foreach (var item in gameObjectCharacters)
        {
            if(item.hasAuthority){
                // if i has authority, is my character
                // in other players will be null
                this.characterCurrent = item;
            }
            
        }
        

    } 

    #endregion
    
}
