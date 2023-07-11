using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownForceTrigger : MonoBehaviour
{
    public void OnTriggerStay(Collider other)
    {
        if (!other.GetComponent<Rigidbody>()) return;
        if (other.CompareTag("MeasureableWeight"))
        {
            other.tag = "DownForce";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Rigidbody>()) return;

        if (other.CompareTag("MeasureableWeight"))
        {
            other.tag = "DownForce";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<Rigidbody>()) return;
        if (!other.CompareTag("DownForce") && !other.CompareTag("MeasureableWeight")) return;
        other.tag = "MeasureableWeight";
    }
}