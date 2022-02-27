using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NetworkAnimator))]
public class Health : NetworkBehaviour
{
    [SyncVar(hook=nameof(handleUpdatedHealth))]
    private float healthPointsActual;
    public float healthPointsMax {get;set;} = 100;

    public Animator animator {get;set;}

    public event Action<float, float> onClientHealthUpdated;

    private void Start() {
        this.healthPointsActual = this.healthPointsMax;
        this.animator = this.GetComponent<Animator>();
    }

    [Server]
    public void damage(float amount){
        if(amount <= 0 || amount > this.healthPointsMax) return;
        this.healthPointsActual -= amount;
    }

    public void handleUpdatedHealth(float valueOld, float valueNew){
        this.healthPointsActual = valueNew;
        onClientHealthUpdated?.Invoke(valueOld,valueNew);

        if(this.animator != null){
            StartCoroutine(receiveDamage(AnimationEnum.ReceiveDamageUp));
            
        }
    }

    IEnumerator receiveDamage(AnimationEnum animationEnum){
        this.animator.Play(animationEnum.ToString());
        yield return new WaitForSeconds(1f);
        this.animator.Play(AnimationEnum.Idle.ToString());
    }



}
