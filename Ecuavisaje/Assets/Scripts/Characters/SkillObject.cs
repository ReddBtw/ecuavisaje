using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SkillObject: NetworkBehaviour{
    public float damage {get;set;}
    public Collider ignoreCollider {get;set;}
    public float attackRange {get;set;}

    public int directionLooking {get;set;} // to left=-1, to right=1


    

}



