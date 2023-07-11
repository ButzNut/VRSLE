using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class RotateLight : MonoBehaviour
{
    public Transform lightTransform;
    public GameObject spotLight1;
    public GameObject spotLight2;
    public float turnSpeed;

    public bool rotate;
    // Update is called once per frame
    void Update()
    {
        if (rotate)
        {
            spotLight1.SetActive(true);
            spotLight2.SetActive(true);
            lightTransform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
        }
        else
        {
            spotLight1.SetActive(false);
            spotLight2.SetActive(false);
        }
    }
}