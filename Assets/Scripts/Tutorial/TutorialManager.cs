using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    public AudioSource aSource;
    public AudioClip[] clips;
    private bool _triggered;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One) && !_triggered)
        {
            StartCoroutine(StartTutorial());
            _triggered = true;
        }
    }

    public IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(5);
        aSource.clip = clips[0];
        aSource.Play();
    }

    public void PlayAudio(int clip)
    {
        aSource.clip = clips[clip];
        aSource.Play();
    }
}