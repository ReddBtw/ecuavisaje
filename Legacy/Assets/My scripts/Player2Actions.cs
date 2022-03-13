using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Actions : MonoBehaviour
{
    public float JumpSpeed = 1.0f;
    public GameObject Player2;
    private Animator Anim;
    private AnimatorStateInfo Player1Layer0;
    private bool HeavyMoving = false;
    public float PunchSlideAmt = 2f;
    public float KickSlideAmt = 1f;
    private bool HeavyKicking = false;
    private AudioSource MyPlayer;
    public AudioClip PunchWoo;
    public AudioClip KickWoo;
    public AudioClip HeavyPunchWoo;
    public AudioClip HeavyKickWoo;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        MyPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Heavy Punch Slide

        if(HeavyMoving == true)
        {
            if (Player2Move.FacingLeftP2 == true)
            {
                Player2.transform.Translate(-PunchSlideAmt * Time.deltaTime, 0, 0);
            }

            if (Player2Move.FacingRightP2 == true)
            {
                Player2.transform.Translate(PunchSlideAmt * Time.deltaTime, 0, 0);
            }

        }

        if (HeavyKicking == true)
        {
            if (Player1Move.FacingRight == true)
            {
                Player2.transform.Translate(KickSlideAmt * Time.deltaTime, 0, 0);
            }

            if (Player1Move.FacingLeft == true)
            {
                Player2.transform.Translate(-KickSlideAmt * Time.deltaTime, 0, 0);
            }

        }


        //Listen to the Animator
        Player1Layer0 = Anim.GetCurrentAnimatorStateInfo(0);

        //Standing attacks

        {
            if (Input.GetButtonDown("Fire1P2"))
            {
                Anim.SetTrigger("LightPunch");
               
                
            }
            if (Input.GetButtonDown("Fire2P2"))
            {
                Anim.SetTrigger("HeavyPunch");
             
            }
            if (Input.GetButtonDown("Fire3P2"))
            {
                Anim.SetTrigger("LightKick");
             
            }
            if (Input.GetButtonDown("JumpP2"))
            {
                Anim.SetTrigger("HeavyKick");
                
            }

            if(Input.GetButtonDown("BlockP2"))
            {
                Anim.SetTrigger("BlockOn");
            }
        }

        if(Player1Layer0.IsTag("Block"))
        {
            if (Input.GetButtonUp("Block"))
            {
                Anim.SetTrigger("BlockOff");
            }
        }

     //Aerial moves

        if (Player1Layer0.IsTag("Jumping"))
        {
            if (Input.GetButtonDown("Jump"))
            {
                Anim.SetTrigger("HeavyKick");
            }
        }

    }

    public void JumpUp()
    {
        Player2.transform.Translate(0, JumpSpeed, 0);
    }

    public void HeavyMove()
    {
        StartCoroutine(PunchSlide());
        Player2.transform.Translate(0, 0, 0);
    }

    public void FlipUp()
    {
        Player2.transform.Translate(0, JumpSpeed, 0);
        Player2.transform.Translate(0.05f, 0, 0);
    }
    public void FlipBack()
    {
        Player2.transform.Translate(0, JumpSpeed, 0);
        Player2.transform.Translate(-0.05f, 0, 0);
    }

    public void PunchWooSound()
    {
        MyPlayer.clip = PunchWoo;
        MyPlayer.Play();
    }

    public void KickWooSound()
    {
        MyPlayer.clip = KickWoo;
        MyPlayer.Play();
    }

    public void HeavyPunchWooSound()
    {
        MyPlayer.clip = HeavyPunchWoo;
        MyPlayer.Play();
    }

    public void HeavyKickWooSound()
    {
        MyPlayer.clip = HeavyKickWoo;
        MyPlayer.Play();
    }
    IEnumerator PunchSlide()
    {
        HeavyMoving = true;
        yield return new WaitForSeconds(0.1f);
        HeavyMoving = false;
    }

    IEnumerator KickSlide()
    {
        HeavyMoving = true;
        yield return new WaitForSeconds(0.1f);
        HeavyMoving = false;
    }
}
