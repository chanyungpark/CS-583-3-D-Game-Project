using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguisherAudio : MonoBehaviour
{
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void StartSpray()
    {
        if (source != null && !source.isPlaying)
        {
            source.Play();
        }
    }

    public void StopSpray()
    {
        if (source != null && source.isPlaying)
        {
            source.Stop();
        }
    }
}

