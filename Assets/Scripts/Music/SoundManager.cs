//SoundManager
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip seagullSound;
    static AudioSource audioSrc;
    // Start is called before the first frame update
    void Start()
    {

        seagullSound = Resources.Load<AudioClip>("SeagullSound");
        audioSrc = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    public static void PlaySound()
    {

        audioSrc.PlayOneShot(seagullSound);

    }
}