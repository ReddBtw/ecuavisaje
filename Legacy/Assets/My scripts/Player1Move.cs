using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Move : MonoBehaviour
{
    private Animator Anim;
    public float WalkSpeed = 0.001f;
    private bool IsJumping = false;
    private AnimatorStateInfo Player1Layer0;
    private bool CanWalkLeft = true;
    private bool CanWalkRight = true;
    public GameObject Player1;
    public GameObject Opponent;
    private Vector3 OppPosition;
    public static bool FacingLeft = true;
    public static bool FacingRight = false;
    public AudioClip LightPunch;
    public AudioClip HeavyPunch;
    public AudioClip LightKick;
    public AudioClip HeavyKick;
    private AudioSource MyPlayer;


    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        MyPlayer = GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Listen to the Animator
        Player1Layer0 = Anim.GetCurrentAnimatorStateInfo(0);

        Vector3 ScreenBounds = Camera.main.WorldToScreenPoint(this.transform.position);

        if (ScreenBounds.x > Screen.width - 200)
        {
            CanWalkLeft = false;
        }
        if (ScreenBounds.x < 200)
        {
            CanWalkRight = false;
        }
        else if (ScreenBounds.x > 100 && ScreenBounds.x < Screen.width - 100)
        {
            CanWalkRight = true;
            CanWalkLeft = true;
        }

        //Get the opponent´s position
        OppPosition = Opponent.transform.position;

        //Facing left or right of the opponent
        if(OppPosition.x > Player1.transform.position.x)
        {
            StartCoroutine(FaceLeft());
        }
        if (OppPosition.x < Player1.transform.position.x)
        {
            StartCoroutine(FaceRight());
        }

        //Flip around to face opponent
        

        //Walking left and right
        if (Player1Layer0.IsTag("Motion"))
        {
            if (Input.GetAxis("HorizontalP1") > 0)
            {
                if (CanWalkRight == true)
                {
                    Anim.SetBool("Forward", true);
                    transform.Translate(WalkSpeed, 0, 0);
                }
            }
            if (Input.GetAxis("HorizontalP1") < 0)
            {
                if (CanWalkLeft == true)
                {
                    Anim.SetBool("Backward", true);
                    transform.Translate(-WalkSpeed, 0, 0);
                }
            }
        }
        if (Input.GetAxis("HorizontalP1") == 0)
        {
            Anim.SetBool("Forward", false);
            Anim.SetBool("Backward", false);
        }
        //Jumping and crouching
        if (Input.GetAxis("VerticalP1") > 0)
        {
            if(IsJumping == false)
            {
                IsJumping = true;
                Anim.SetTrigger("Jump");
                StartCoroutine(JumpPause());
            }
        }
        if (Input.GetAxis("VerticalP1") < 0)
        {
            Anim.SetBool("Crouch", true);
        }
        if (Input.GetAxis("VerticalP1") == 0)
        {
            Anim.SetBool("Crouch", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FistLight"))
        {
            Anim.SetTrigger("HeadReact");
            MyPlayer.clip = LightPunch;
            MyPlayer.Play();
        }
        if (other.gameObject.CompareTag("FistHeavy"))
        {
            Anim.SetTrigger("BigReact");
            MyPlayer.clip = HeavyPunch;
            MyPlayer.Play();
        }
        if (other.gameObject.CompareTag("KickHeavy"))
        {
            Anim.SetTrigger("BigReact");
            MyPlayer.clip = HeavyKick;
            MyPlayer.Play();
        }
        if (other.gameObject.CompareTag("KickLight"))
        {
            Anim.SetTrigger("HeadReact");
            MyPlayer.clip = LightKick;
            MyPlayer.Play();
        }

    }
    IEnumerator JumpPause()
    {
        yield return new WaitForSeconds(1.0f);
        IsJumping = false;
    }

    

    IEnumerator FaceLeft()
    {
        if (FacingLeft == true)
        {
            FacingLeft = false;
            FacingRight = true;
            yield return new WaitForSeconds(0.15f);
            Player1.transform.Rotate(0, 180, 0);
            Anim.SetLayerWeight(1, 1);
        }
    }

    IEnumerator FaceRight()
    {
        if (FacingRight == true)
        {
            FacingRight = false;
            FacingLeft = true;
            yield return new WaitForSeconds(0.15f);
            Player1.transform.Rotate(0, -180, 0);
            Anim.SetLayerWeight(1, 0);
        }
    }
}
