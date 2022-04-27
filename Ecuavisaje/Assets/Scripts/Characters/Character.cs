using System.Collections;
using System.Collections.Generic;
using UnityEngine;
﻿using System;

public enum CharacterEnum
{
    None,
    Lasso,
    Conserje,
    RonAlkonso
}

public enum AnimationEnum
{
    Idle, WalkForward, WalkBackward, Jump, Bend, Sweep, Punch1, Punch2,
    Kick1, Kick2, Block1, GettingUp, KnockedOut,
    ReceiveDamageUp, Stunned, Special1, Special2,
    Ultimate, Uppercut
}

public enum SkillType
{
    normal, throw_object, invocable
}

[Serializable]
 public struct Skill {
    public SkillType skillType;
    public GameObject gameObjectPrefab;
    public AudioClip audio;
    public float damage;
    public float attackRange;
    public Vector3 startPositionOffset; // position to sum, relative to character
}

[CreateAssetMenu(menuName = "Character")]
public class Character: ScriptableObject
{
    [SerializeField] public Sprite icon;
    [SerializeField] public CharacterEnum characterEnum;
    [SerializeField] public GameObject characterPrefab;
    [SerializeField] public string description;
    [SerializeField] public Skill punch1;
    [SerializeField] public Skill special1;
    [SerializeField] public Skill special2;
    [SerializeField] public Skill ultimate;


}
