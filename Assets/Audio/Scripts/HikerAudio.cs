using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HikerAudio : MonoBehaviour
{
    [Header("Audio Sources")]
    private AudioSource source;

    [Header("Clips")]
    public AudioClip panicLoop;
    public AudioClip rescuedClip;

    private bool isPanicking = false;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void StartPanic()
    {
        if (isPanicking) return;

        isPanicking = true;
        source.clip = panicLoop;
        source.loop = true;
        source.Play();
    }

    public void StopPanic()
    {
        if (!isPanicking) return;

        isPanicking = false;
        source.Stop();
        source.loop = false;
    }

    public void PlayRescued()
    {
        StopPanic();
        source.PlayOneShot(rescuedClip);
    }
}

