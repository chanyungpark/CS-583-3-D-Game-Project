using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class HikerAudio : MonoBehaviour
{
    private AudioSource source;
    private Coroutine panicRoutine;

    [Header("Clips")]
    public AudioClip panicClip;
    public AudioClip rescuedClip;

    [Header("Timing")]
    public float panicInterval = 8f;

    private bool isPanicking = false;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartPanic();
    }

    public void StartPanic()
    {
        if (isPanicking || panicClip == null) return;

        isPanicking = true;
        panicRoutine = StartCoroutine(PanicLoop());
    }

    public void StopPanic()
    {
        if (!isPanicking) return;

        isPanicking = false;

        if (panicRoutine != null)
        {
            StopCoroutine(panicRoutine);
            panicRoutine = null;
        }

        source.Stop();
    }

    private IEnumerator PanicLoop()
    {
        while (isPanicking)
        {
            source.PlayOneShot(panicClip);
            yield return new WaitForSeconds(panicInterval);
        }
    }

    public void PlayRescued()
    {
        StopPanic();

        if (rescuedClip != null)
        {
            source.PlayOneShot(rescuedClip);
        }
    }
}

