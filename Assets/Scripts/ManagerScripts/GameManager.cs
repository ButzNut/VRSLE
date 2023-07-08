using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Current Game Phase")] public int currentPhase;


    [Header("Start Sequence")] private float _timer;
    public float timeToReach;
    public GameObject introductionHologram;
    public VideoPlayer introductionVideoPlayer;


    [Header("Puzzle 1")] public GameObject puzzle1Hologram;
    public GameObject starShip;
    public GameObject shipController;
    public GameObject dockingStation;
    
    
    // Start is called before the first frame
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        
        OVRManager.SetSpaceWarp(true); // Enable space warp
        introductionVideoPlayer.loopPointReached += EndReached;
    }

    // Update is called once per frame
    void Update()
    {
        GamePhase();
    }

    void GamePhase()
    {
        switch (currentPhase)
        {
            case 0:
                StartIntroduction();
                break;
            case 1:
                StartPuzzle1();
                break;
            case 2:
                break;
        }
    }

    void StartIntroduction()
    {
        _timer += Time.deltaTime;

        if (!introductionVideoPlayer.isPlaying && _timer >= timeToReach)
        {
            introductionVideoPlayer.Play();
            introductionHologram.SetActive(true);
            introductionVideoPlayer.loopPointReached += EndReached;
        }
    }

    void StartPuzzle1()
    {
        puzzle1Hologram.SetActive(true);
        starShip.SetActive(true);
        shipController.SetActive(true);
    }

    void EndReached(VideoPlayer vp)
    {
        Debug.Log("Video End Reached");
        introductionHologram.SetActive(false);
        currentPhase = 1;
    }
}