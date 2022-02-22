using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2MoveRestrict : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("P1Left"))
        {
            Player2Move.WalkLeft = false;
        }

        if (other.gameObject.CompareTag("P1Right"))
        {
            Player2Move.WalkRight = false;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("P1Left"))
        {
            Player2Move.WalkLeft = true;
        }

        if (other.gameObject.CompareTag("P1Right"))
        {
            Player2Move.WalkRight = false;
        }

    }

}
