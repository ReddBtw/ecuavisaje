using System.Collections;
using System.Collections.Generic;
using UnityEngine;
﻿using System;

public enum CharacterEnum
{
    None,
    Lasso    
}

[CreateAssetMenu(menuName = "Character")]
public class Character: ScriptableObject
{
    [SerializeField] public Sprite icon;
    [SerializeField] public CharacterEnum characterEnum;
    [SerializeField] public GameObject characterPrefab;
    [SerializeField] public string description;

}
