using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoors : MonoBehaviour
{
    public static OpenDoors Instance;

    public GameObject door1;
    public GameObject door2;

    public Transform door1Open;
    public Transform door2Open;
    public Transform door1Closed;
    public Transform door2Closed;
    public float speed;

    public bool openDoors;

    public AudioSource doorOpenSound;
    public AudioSource doorOpenSound2;

    private void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (openDoors)
        {
            door1.transform.position =
                Vector3.MoveTowards(door1.transform.position, door1Open.position, speed * Time.deltaTime);
            door2.transform.position =
                Vector3.MoveTowards(door2.transform.position, door2Open.position, speed * Time.deltaTime);
        }
        else
        {
            door1.transform.position =
                Vector3.MoveTowards(door1.transform.position, door1Closed.position, speed * Time.deltaTime);
            door2.transform.position =
                Vector3.MoveTowards(door2.transform.position, door2Closed.position, speed * Time.deltaTime);
        }
    }

    
}