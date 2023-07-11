using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpForceTrigger : MonoBehaviour
{
    
    
    private void OnTriggerStay(Collider other)
    {
        if(!other.GetComponent<Rigidbody>()) return;
        
        if (other.CompareTag("MeasureableWeight"))
        {
            other.tag = "UpForce";
        }
        else if (other.CompareTag("UpForce"))
        {
            other.GetComponent<Rigidbody>().useGravity = false;
            other.GetComponent<Rigidbody>().AddForce(-Physics.gravity, ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.GetComponent<Rigidbody>()) return;
        if (other.CompareTag("MeasureableWeight"))
        {
            other.tag = "UpForce";
        }
        else if (other.CompareTag("UpForce"))
        {
            other.GetComponent<Rigidbody>().useGravity = false;
            other.GetComponent<Rigidbody>().AddForce(-Physics.gravity, ForceMode.Acceleration);
        }
      
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.GetComponent<Rigidbody>()) return;
        if(!other.CompareTag("UpForce") && !other.CompareTag("MeasureableWeight")) return;
        other.tag = "MeasureableWeight";
        other.GetComponent<Rigidbody>().useGravity = true;
        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}