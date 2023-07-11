using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceCalculator : MonoBehaviour
{
    [SerializeField] private float totalForce;

    private Rigidbody rigid;
    public List<Rigidbody> rbDown = new List<Rigidbody>();
    public List<Rigidbody> rbUp = new List<Rigidbody>();

    public float moveScaleTimer;

    public MeshRenderer mesh;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (totalForce != 0)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition,
                new Vector3(transform.localPosition.x,
                    Mathf.Clamp(-totalForce + transform.localPosition.y, 0.75f, 1.25f), transform.localPosition.z),
                moveScaleTimer);
        }
        else if (totalForce == 0)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition,
                new Vector3(transform.localPosition.x,
                    1, transform.localPosition.z),
                moveScaleTimer);
        }


        if (totalForce == 0 && rbDown.Count > 0 && rbUp.Count > 0 && transform.localPosition.y <= 1.05f &&
            transform.localPosition.y >= 0.95f)
        {
            mesh.material.SetColor("_EmissionColor", Color.green);
        }
        else
            mesh.material.SetColor("_EmissionColor", Color.red);
    }

    void UpdateMass()
    {
        float downForce = 0;
        float upForce = 0;

        foreach (var rb in rbDown)
        {
            downForce += rb.mass;
        }

        foreach (var rb in rbUp)
        {
            upForce += rb.mass;
        }

        totalForce = downForce - upForce;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DownForce"))
        {
            rbDown.Add(other.gameObject.GetComponent<Rigidbody>());
        }

        if (other.gameObject.CompareTag("UpForce"))
        {
            rbUp.Add(other.gameObject.GetComponent<Rigidbody>());
        }

        UpdateMass();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("DownForce") || other.gameObject.CompareTag("MeasureableWeight"))
        {
            rbDown.Remove(other.gameObject.GetComponent<Rigidbody>());
        }

        if (other.gameObject.CompareTag("UpForce") || other.gameObject.CompareTag("MeasureableWeight"))
        {
            rbUp.Remove(other.gameObject.GetComponent<Rigidbody>());
        }

        UpdateMass();
    }
}