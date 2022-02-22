using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Actions : MonoBehaviour
{
    public float JumpSpeed = 1.0f;
    public GameObject Player1;
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
    public AudioClip JumpFront;
    public AudioClip JumpSide;

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
            if (Player1Move.FacingRight == true)
            {
                Player1.transform.Translate(PunchSlideAmt * Time.deltaTime, 0, 0);
            }

            if (Player1Move.FacingLeft == true)
            {
                Player1.transform.Translate(-PunchSlideAmt * Time.deltaTime, 0, 0);
            }

        }

        if (HeavyKicking == true)
        {
            if (Player1Move.FacingRight == true)
            {
                Player1.transform.Translate(KickSlideAmt * Time.deltaTime, 0, 0);
            }

            if (Player1Move.FacingLeft == true)
            {
                Player1.transform.Translate(-KickSlideAmt * Time.deltaTime, 0, 0);
            }

        }



        //Listen to the Animator
        Player1Layer0 = Anim.GetCurrentAnimatorStateInfo(0);

        //Standing attacks

        {
            if (Input.GetButtonDown("Fire1"))
            {
                Anim.SetTrigger("LightPunch");
               
                
            }
            if (Input.GetButtonDown("Fire2"))
            {
                Anim.SetTrigger("HeavyPunch");
             
            }
            if (Input.GetButtonDown("Fire3"))
            {
                Anim.SetTrigger("LightKick");
             
            }
            if (Input.GetButtonDown("Jump"))
            {
                Anim.SetTrigger("HeavyKick");
                
            }

            if(Input.GetButtonDown("Block"))
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
        Player1.transform.Translate(0, JumpSpeed, 0);
    }

    public void HeavyMove()
    {
        StartCoroutine(PunchSlide());
        Player1.transform.Translate(0, 0, 0);
    }

    public void FlipUp()
    {
        Player1.transform.Translate(0, JumpSpeed, 0);
        Player1.transform.Translate(0.04f, 0, 0);
    }
    public void FlipBack()
    {
        Player1.transform.Translate(0, JumpSpeed, 0);
        Player1.transform.Translate(-0.04f, 0, 0);
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

    public void JumpFrontSound()
    {
        MyPlayer.clip = JumpFront;
        MyPlayer.Play();
    }

    public void JumpSideSound()
    {
        MyPlayer.clip = JumpSide;
        MyPlayer.Play();
    }

    IEnumerator PunchSlide()
    {
        HeavyMoving = true;
        yield return new WaitForSeconds(0.2f);
        HeavyMoving = false;
    }

    IEnumerator KickSlide()
    {
        HeavyMoving = true;
        yield return new WaitForSeconds(0.1f);
        HeavyMoving = false;
    }
}
