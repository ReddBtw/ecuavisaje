using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemAudio : MonoBehaviour
{
    public static void playAtPosition(AudioClip audioClip, Vector3 position){
        AudioSource.PlayClipAtPoint(audioClip, position);
    }
}
