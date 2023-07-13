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
    public ShipHandler shipController;
    public GameObject dockingStation;

    [Header("Puzzle 2")] public GameObject puzzle2Hologram;
    public GameObject canvasError;
    public GameObject canvasSuccess;
    public DialChange dialChange;
    public MeshRenderer BatteryStatus;


    [Header("Puzzle 3")] public GameObject puzzle3Hologram;
    public GameObject spaceContainer;
    public Transform spaceContainerSpawn;
    bool spawned = false;
    public Transform spaceContainerDestination;
    public RotateLight light1, light2;
    public AudioSource alarm1, alarm2;

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

        // OVRManager.SetSpaceWarp(true); // Enable space warp
        introductionVideoPlayer.loopPointReached += EndReached;
        introductionVideoPlayer.prepareCompleted += PrepareCompleted;
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
            case 3:
                StartPuzzle3();
                break;
        }
    }

    void StartIntroduction()
    {
        _timer += Time.deltaTime;

        if (!introductionVideoPlayer.isPlaying && _timer >= timeToReach)
        {
            if (introductionVideoPlayer.isPrepared) ;
            introductionVideoPlayer.Play();
            introductionVideoPlayer.Prepare();
            introductionVideoPlayer.loopPointReached += EndReached;
        }
    }

    void StartPuzzle1()
    {
        puzzle1Hologram.SetActive(true);
        shipController.enabled = true;
        dockingStation.SetActive(true);

        if (dockingStation.GetComponent<DockShip>().docking)
        {
            StartCoroutine(Puzzle2Starter());
            puzzle1Hologram.SetActive(false);
        }
    }

    void StartPuzzle2()
    {
        float intensity = Mathf.PingPong(Time.time, 1.5f);
        shipController.enabled = false;
        BatteryStatus.materials[1].SetColor("_EmissionColor", Color.red * intensity);
        canvasError.SetActive(true);
        canvasSuccess.SetActive(false);
        dialChange.enabled = true;
        // puzzle2Hologram.SetActive(true);
    }

    public void StartPuzzle3()
    {
        BatteryStatus.materials[1].SetColor("_EmissionColor", new Color(0, 234, 255));
        canvasError.SetActive(true);
        canvasSuccess.SetActive(false);
        dialChange.enabled = true;
    }

    void PrepareCompleted(VideoPlayer vp)
    {
        introductionVideoPlayer.GetComponent<MeshRenderer>().enabled = true;
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
        light1.rotate = true;
        alarm1.Play();
        light2.rotate = true;
        alarm2.Play();
        yield return new WaitForSeconds(5);
        if (!spawned)
        {
            spawned = true;
            spaceContainer = Instantiate(spaceContainer, spaceContainerSpawn.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(15);
        light1.rotate = false;
        alarm1.Stop();
        light2.rotate = false;
        alarm2.Stop();
        yield return new WaitForSeconds(4);
        currentPhase = 2;
    }
}