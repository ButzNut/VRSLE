using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpForceTrigger : MonoBehaviour
{
    public EqualForceScale equalForceScale;
    
    
    private void OnTriggerStay(Collider other)
    {
        if(!other.GetComponent<Rigidbody>()) return;
        
        if (other.CompareTag("MeasureableWeight"))
        {
            other.tag = "UpForce";
            equalForceScale._impulseUpPerRigidBody.Add(other.GetComponent<Rigidbody>(), 0);
        }
        else if (other.CompareTag("UpForce"))
        {
            other.GetComponent<Rigidbody>().useGravity = false;
            other.GetComponent<Rigidbody>().AddForce(-Physics.gravity, ForceMode.Acceleration);
        }
        else if (other.CompareTag("UpForce") && !equalForceScale._impulseUpPerRigidBody.ContainsKey(other.GetComponent<Rigidbody>()))
        {
            equalForceScale._impulseUpPerRigidBody.Add(other.GetComponent<Rigidbody>(), 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.GetComponent<Rigidbody>()) return;
        if (other.CompareTag("MeasureableWeight"))
        {
            other.tag = "UpForce";
            equalForceScale._impulseUpPerRigidBody.Add(other.GetComponent<Rigidbody>(), 0);
        }
        else if (other.CompareTag("UpForce"))
        {
            other.GetComponent<Rigidbody>().useGravity = false;
            other.GetComponent<Rigidbody>().AddForce(-Physics.gravity, ForceMode.Acceleration);
        }
        else if (other.CompareTag("UpForce") && !equalForceScale._impulseUpPerRigidBody.ContainsKey(other.GetComponent<Rigidbody>()))
        {
            equalForceScale._impulseUpPerRigidBody.Add(other.GetComponent<Rigidbody>(), 0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.GetComponent<Rigidbody>()) return;
        if(!other.CompareTag("UpForce") && !other.CompareTag("MeasureableWeight")) return;
        other.tag = "MeasureableWeight";
        equalForceScale._impulseUpPerRigidBody.Remove(other.GetComponent<Rigidbody>());
        other.GetComponent<Rigidbody>().useGravity = true;
        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}