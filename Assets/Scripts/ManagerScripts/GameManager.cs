using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public bool startFirstPuzzle, firstPuzzleDone;
    public bool cargoDelivered;
    public bool secondPuzzleDone;


    public float timer, timeToReach;

    [Header("Start Sequence")] public bool introductionStart;
    public GameObject introductionHologram;
    public VideoPlayer introductionVideoPlayer;


    [Header("Puzzle 1")] public GameObject puzzle1Hologram;

    // Start is called before the first frame
    void Awake()
    {
       // OVRManager.SetSpaceWarp(true); // Enable space warp
        //introductionVideoPlayer.loopPointReached += EndReached;
        introductionStart = true;

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeToReach)
        {
            StartIntroduction();
        }
    }

    void StartIntroduction()
    {
        if (!introductionVideoPlayer.isPlaying && introductionStart)
        {
            introductionHologram.SetActive(true);
            introductionVideoPlayer.Play();
            introductionVideoPlayer.loopPointReached += EndReached;
            introductionStart = false;
        }
    }


    void EndReached(VideoPlayer vp)
    {
        Debug.Log("Video End Reached");
        introductionHologram.SetActive(false);
    }
}