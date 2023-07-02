using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool introductionStart;
    public bool firstPuzzleDone;
    public bool cargoDelivered;
    public bool secondPuzzleDone;
    
    // Start is called before the first frame update
    void Awake()
    {
        OVRManager.SetSpaceWarp(true); // Enable space warp
    }

    // Update is called once per frame
    void Update()
    {
        if (introductionStart)
        {
            StartIntroduction();
        }
    }
    
    void StartIntroduction()
    {
        
    }
    
}
