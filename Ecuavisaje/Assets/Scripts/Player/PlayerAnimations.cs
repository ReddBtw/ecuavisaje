using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AnimationEnum
{
    Idle, Walk, Jump, Bend, Sweep, Punch1, PUnch2,
    Kick1, Kick2, Block1, GettingUp, KnockedOut,
    ReceiveDamageUp, Stunned
}

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] PlayerActions playerActions;

    private Animator animator;
    private AnimationEnum stateCurrent = AnimationEnum.Idle;

    void Start(){
        this.animator = playerActions.getAnimator();
    }

    private void stopAttacking(){
        this.playerActions.is_attacking = false;
        this.playerActions.is_punch1_pressed = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(this.animator == null){
            // todo: optimize
            this.animator = playerActions.getAnimator();
            return;
        }

        if(this.playerActions.is_grounded){

            if(this.playerActions.is_attacking){
                if(this.playerActions.is_punch1_pressed){
                    changeStateAnimation(AnimationEnum.Punch1);
                    Invoke("stopAttacking", 0.7f);
                }
            }
            else{
                if(this.playerActions.is_walking){
                    changeStateAnimation(AnimationEnum.Walk);
                }
                else if(this.playerActions.is_bend_pressed){
                    changeStateAnimation(AnimationEnum.Bend);
                }
                else{
                    changeStateAnimation(AnimationEnum.Idle);
                }
            }
            
            
        }
        else{
            if(this.playerActions.is_jump_pressed){
                changeStateAnimation(AnimationEnum.Jump);
                this.playerActions.is_jump_pressed = false;
            }
        }        
    }

    private void changeStateAnimation(AnimationEnum stateNew){

        if(this.stateCurrent == stateNew) return;

        this.animator.Play(stateNew.ToString());

        this.stateCurrent = stateNew;

    }
}
