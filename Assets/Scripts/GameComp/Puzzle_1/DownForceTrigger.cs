using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownForceTrigger : MonoBehaviour
{
    public EqualForceScale equalForceScale;

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MeasureableWeight"))
        {
            other.tag = "DownForce";
            equalForceScale._impulseDownPerRigidBody.Add(other.GetComponent<Rigidbody>(), 0);
        }
        else if (other.CompareTag("DownForce") && !equalForceScale._impulseDownPerRigidBody.ContainsKey(other.GetComponent<Rigidbody>()))
        {
            equalForceScale._impulseDownPerRigidBody.Add(other.GetComponent<Rigidbody>(), 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MeasureableWeight"))
        {
            other.tag = "DownForce";
            equalForceScale._impulseDownPerRigidBody.Add(other.GetComponent<Rigidbody>(), 0);
        }
        else if (other.CompareTag("DownForce") && !equalForceScale._impulseDownPerRigidBody.ContainsKey(other.GetComponent<Rigidbody>()))
        {
            equalForceScale._impulseDownPerRigidBody.Add(other.GetComponent<Rigidbody>(), 0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.tag = "MeasureableWeight";
        equalForceScale._impulseDownPerRigidBody.Remove(other.GetComponent<Rigidbody>());
    }
}