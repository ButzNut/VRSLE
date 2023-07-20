using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Video;
using UnityEngine.XR.Content.Interaction;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Current Game Phase")] public int currentPhase;
    [Header("General")] public OcclusionPortal holoPortal;
    public MeshRenderer windowBlocker;
    public bool blockWindow = true;
    public float alpha = 255;
    private float intensity;
    public AudioSource boardMessage;
    public AudioClip[] boardMessages;

    public MeshRenderer hologram;
    public VideoPlayer holoPlayer;
    public VideoClip[] holoClips;
    public AudioSource holoAudioSource;
    public AudioClip[] holoAudioClips;


    [Header("Start Sequence")] [SerializeField]
    private float _timer;

    public float timeToReach;
    private bool introStart = false;


    [Header("Puzzle 1")] public GameObject shipMapHologram;
    public GameObject spaceShip;
    public ShipHandler shipController;
    public GameObject dockingStation;
    public bool puzzle1Solved = false;

    [Header("Puzzle 2")] private bool _hologram2Start;
    public GameObject canvasError;
    public GameObject canvasSuccess;
    public DialChange dialChange;
    public MeshRenderer batteryStatus;
    public AudioSource batteryAudioSource;
    public XRPushButton batteryButton;
    public bool puzzle2Solved = false;

    [Header("Puzzle 3")] private bool _hologram3Start;
    public GameObject spaceContainer;
    public Transform spaceContainerSpawn;
    bool _spawned = false;
    public Transform spaceContainerDestination;
    public Transform spaceContainerDespawn;
    public RotateLight light1, light2;
    public AudioSource alarm1, alarm2;
    public bool puzzle3Solved = false;
    private bool _containerLoaded = false;
    public ForceCalculator[] forceCalculators;
    public GameObject[] powerCrystalsBoxes;
    public XRPushButton ejectButton;


    [Header("Puzzle 4")] public Transform shipPoint1;
    public Transform shipPoint2;
    private int shipPhase;
    public MeshRenderer[] lights;
    public MeshRenderer lightTank;
    public GameObject CoreStableUI;
    public GameObject CoreUnstableUI;
    public bool puzzle4Solved = false;


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

        if (OVRPlugin.GetSystemHeadsetType() == OVRPlugin.SystemHeadset.Oculus_Quest_2)
        {
            OVRManager.SetSpaceWarp(true); // Enable space warp
            OVRManager.display.displayFrequency = 72.0f; // Set the display refresh rate to 72Hz
            OVRManager.foveatedRenderingLevel = OVRManager.FoveatedRenderingLevel.Medium; // Enable foveated rendering
        }

        holoPlayer.loopPointReached += EndReached;
    }

    void GetHologramReady(int currentPhase)
    {
        blockWindow = true;
        holoPlayer.clip = holoClips[currentPhase];
        holoAudioSource.clip = holoAudioClips[currentPhase];
        hologram.enabled = true;
        holoPlayer.Play();
        holoAudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        GamePhase();

        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(Puzzle3Starter());
        }

        intensity = Mathf.PingPong(Time.time, 1.5f);

        if (spaceContainer != null)
        {
            if (_spawned)
            {
                spaceContainer.transform.position = Vector3.MoveTowards(spaceContainer.transform.position,
                    spaceContainerDestination.position, 0.7f * Time.deltaTime);
            }
            else if (_containerLoaded)
            {
                spaceContainer.transform.position = Vector3.MoveTowards(spaceContainer.transform.position,
                    spaceContainerDespawn.position, 0.7f * Time.deltaTime);
            }
        }

        if (shipPhase == 1)
        {
            spaceShip.transform.position = Vector3.MoveTowards(spaceShip.transform.position,
                shipPoint1.position, 5f * Time.deltaTime);
        }
        else if (shipPhase == 2)
        {
            spaceShip.transform.rotation = Quaternion.RotateTowards(spaceShip.transform.rotation,
                shipPoint2.rotation, 50f * Time.deltaTime);
        }
        else if (shipPhase == 3)
        {
            spaceShip.transform.position = Vector3.MoveTowards(spaceShip.transform.position,
                shipPoint2.position, 50f * Time.deltaTime);
        }


        if (!blockWindow)
        {
            alpha -= Time.deltaTime * 40;
            if (alpha > 0)
            {
                windowBlocker.material.SetColor("_BaseColor", new Color(0, 0, 0, alpha / 255));
                holoPortal.open = true;
            }
        }
        else
        {
            alpha += Time.deltaTime * 90;
            if (alpha < 255)
            {
                windowBlocker.material.SetColor("_BaseColor", new Color(0, 0, 0, alpha / 255));
                holoPortal.open = false;
            }
        }
    }

    void GamePhase()
    {
        alpha = Mathf.Clamp(alpha, 0, 255);
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
            case 4:
                StartPuzzle4();
                break;
        }
    }

    void StartIntroduction()
    {
        _timer += Time.deltaTime;
        if (!holoPlayer.isPlaying && _timer >= timeToReach && !introStart)
        {
            GetHologramReady(currentPhase);
            introStart = true;
        }
    }

    void StartPuzzle1()
    {
        shipController.enabled = true;
        dockingStation.SetActive(true);


        if (dockingStation.GetComponent<DockShip>().docking && !puzzle1Solved)
        {
            puzzle1Solved = true;
            StartCoroutine(Puzzle2Starter());
        }
    }

    void StartPuzzle2()
    {
        shipController.enabled = false;
        batteryStatus.materials[1].SetColor("_EmissionColor", Color.red * intensity);
        canvasError.SetActive(true);
        canvasSuccess.SetActive(false);
        dialChange.enabled = true;
    }

    public void StartPuzzle3()
    {
        batteryStatus.materials[1].SetColor("_EmissionColor", new Color(0, 234, 255) / 50);
        canvasError.SetActive(true);
        canvasSuccess.SetActive(false);
        dialChange.enabled = false;
        if (!_hologram3Start)
        {
            GetHologramReady(currentPhase - 1);
            _hologram3Start = true;
        }

        if (forceCalculators.Length < 1)
        {
            forceCalculators = FindObjectsOfType<ForceCalculator>();
        }

        bool scale1 = forceCalculators[0].scaleIsSet;
        bool scale2 = forceCalculators[1].scaleIsSet;

        if (scale1 && scale2 && !puzzle3Solved)
        {
            CapsuleLoadedAudio();
            puzzle3Solved = true;
        }
    }

    public void StartPuzzle4()
    {
        if (!puzzle4Solved)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].materials[0].SetColor("_EmissionColor", Color.red * Mathf.PingPong(Time.time, 1f));
            }

            lightTank.materials[0].SetColor("_EmissionColor", Color.red * Mathf.PingPong(Time.time, 0.5f));

            CoreStableUI.SetActive(false);
            CoreUnstableUI.SetActive(true);
        }
        else
        {
            CoreStableUI.SetActive(true);
            CoreUnstableUI.SetActive(false);
            
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].materials[0].SetColor("_EmissionColor", Color.white * 2);
            }

            lightTank.materials[0].SetColor("_EmissionColor", Color.cyan * 2);

        }
    }

    void EndReached(VideoPlayer vp)
    {
        Debug.Log("Video End Reached");
        hologram.enabled = false;
        blockWindow = false;
        if (currentPhase == 0)
        {
            currentPhase = 1;
            shipMapHologram.SetActive(true);
        }
    }

    IEnumerator Puzzle2Starter()
    {
        shipMapHologram.SetActive(false);
        spaceShip.GetComponent<BoxCollider>().isTrigger = true;
        spaceShip.GetComponent<Rigidbody>().isKinematic = true;
        alarm2.Play();
        alarm1.Play();
        light1.rotate = true;
        light2.rotate = true;
        yield return new WaitForSeconds(5);
        if (!_spawned)
        {
            _spawned = true;
            spaceContainer = Instantiate(spaceContainer, spaceContainerSpawn.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(15);
        light1.rotate = false;
        light2.rotate = false;
        alarm1.Stop();
        alarm2.Stop();
        yield return new WaitForSeconds(1);
        currentPhase = 2;
        batteryAudioSource.Play();
        yield return new WaitForSeconds(1);
        boardMessage.clip = boardMessages[0];
        boardMessage.Play();
        yield return new WaitForSeconds(5);
        if (!_hologram2Start)
        {
            GetHologramReady(currentPhase - 1);
            _hologram2Start = true;
        }
    }

    public IEnumerator Puzzle3Starter()
    {
        batteryButton.enabled = false;
        boardMessage.clip = boardMessages[1];
        boardMessage.Play();
        yield return new WaitForSeconds(5);
        OpenDoors.Instance.doorOpenSound.Play();
        OpenDoors.Instance.doorOpenSound2.Play();
        OpenDoors.Instance.openDoors = true;
        puzzle2Solved = true;
    }

    IEnumerator Puzzle4Starter()
    {
        ejectButton.enabled = false;
        alarm2.Play();
        alarm1.Play();
        light1.rotate = true;
        light2.rotate = true;
        OpenDoors.Instance.doorOpenSound.Play();
        OpenDoors.Instance.doorOpenSound2.Play();
        OpenDoors.Instance.openDoors = false;
        foreach (var p in powerCrystalsBoxes)
        {
            p.GetComponent<Rigidbody>().isKinematic = true;
            p.transform.parent = spaceContainer.transform;
        }

        yield return new WaitForSeconds(5);
        _spawned = false;
        _containerLoaded = true;
        yield return new WaitForSeconds(10);
        Destroy(spaceContainer.gameObject);
        yield return new WaitForSeconds(2);
        shipPhase = 1;
        yield return new WaitForSeconds(5);
        shipPhase = 2;
        yield return new WaitForSeconds(5);
        shipPhase = 3;
        yield return new WaitForSeconds(5);
        alarm2.Stop();
        alarm1.Stop();
        light1.rotate = false;
        light2.rotate = false;
        yield return new WaitForSeconds(3);
        light1.rotate = true;
        light2.rotate = true;
        MoveStabiliser.Instance.speed = 10;
        boardMessage.clip = boardMessages[3];
        boardMessage.Play();
        currentPhase = 4;
    }

    public void LaunchCapsule()
    {
        if (puzzle3Solved)
            StartCoroutine(Puzzle4Starter());
    }

    public void CapsuleLoadedAudio()
    {
        boardMessage.clip = boardMessages[2];
        boardMessage.Play();
    }
}