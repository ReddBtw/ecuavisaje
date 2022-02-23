using System.Collections;
using System.Collections.Generic;
using UnityEngine;
﻿using System;
using Mirror;

public class Character : NetworkBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private Sprite icon;
    [SerializeField] private int id;


    public static event Action<Character> OnCharacterServerSpawned;
    public static event Action<Character> OnCharacterServerDespawned;

    public static event Action<Character> OnAuthoritySpawned;
    public static event Action<Character> OnAuthorityDespawned;

    #region GettersSetters
        public GameObject getModel(){
            return this.model;
        }
        public int getId(){
            return this.id;
        }
    #endregion

    #region Server

    public override void OnStartServer()
    {
        Character.OnCharacterServerSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        Character.OnCharacterServerDespawned?.Invoke(this);
    }

    #endregion

    #region Client

    public override void OnStartAuthority()
    {
        Character.OnAuthoritySpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!hasAuthority) { return; }

        Character.OnAuthorityDespawned?.Invoke(this);
    }

    #endregion


}
