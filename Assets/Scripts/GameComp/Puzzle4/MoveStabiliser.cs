using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStabiliser : MonoBehaviour
{
    public static MoveStabiliser Instance;
    
    public GameObject stabiliser;
    public Transform stabiliserUp;
    public Transform stabiliserDown;
    public float speed;
    [SerializeField] private float _distance;
    public bool stabiliserUpBool;

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(stabiliser.transform.localPosition, stabiliserUp.localPosition) < _distance)
        {
            stabiliserUpBool = false;
        }
        else if (Vector3.Distance(stabiliser.transform.localPosition, stabiliserDown.localPosition) < _distance)
        {
            stabiliserUpBool = true;
        }

        stabiliser.transform.position = Vector3.Slerp(stabiliser.transform.position,
            stabiliserUpBool ? stabiliserUp.position : stabiliserDown.position, speed * Time.deltaTime);
    }
}