using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
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
    [FormerlySerializedAs("starShip")] public GameObject spaceShip;
    public GameObject shipController;
    public GameObject dockingStation;
    
    [Header("Puzzle 2")] 
    public GameObject puzzle2Hologram;
    public GameObject spaceContainer;
    public Transform spaceContainerSpawn;
    bool spawned = false;
    public Transform spaceContainerDestination;
    public RotateLight light1, light2;
    
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

        if (spawned)
        {
            spaceContainer.transform.position = Vector3.MoveTowards(spaceContainer.transform.position,
                spaceContainerDestination.position, 0.7f * Time.deltaTime);
        }
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
                StartPuzzle2();
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
        shipController.SetActive(true);
        spaceShip.SetActive(true);
        dockingStation.SetActive(true);

        if (dockingStation.GetComponent<DockShip>().docking)
        {
            StartCoroutine(Puzzle2Starter());
        }
    }
    
    public void StartPuzzle2()
    {
        //puzzle1Hologram.SetActive(false);
        shipController.SetActive(false);
    }

    void EndReached(VideoPlayer vp)
    {
        Debug.Log("Video End Reached");
        introductionHologram.SetActive(false);
        currentPhase = 1;
    }
    
    IEnumerator Puzzle2Starter()
    {
        spaceShip.GetComponent<BoxCollider>().isTrigger = true;
        spaceShip.GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(5);
        currentPhase = 2;
        light1.rotate = true;
        light2.rotate = true;
        if (!spawned)
        {
            spawned = true;
            spaceContainer = Instantiate(spaceContainer, spaceContainerSpawn.position, Quaternion.identity);
        }
        
        yield return new WaitForSeconds(15);
        OpenDoors.Instance.openDoors = true;
        yield return new WaitForSeconds(5);
        light1.rotate = false;
        light2.rotate = false;
    }
    
    
}