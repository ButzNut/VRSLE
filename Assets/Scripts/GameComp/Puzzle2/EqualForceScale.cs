using System.Collections.Generic;
using UnityEngine;

public class EqualForceScale : MonoBehaviour
{
    float forceToMass;

    public float combinedForce;
    public float calculatedMass;

    public int registeredRigidbodies;

    public Dictionary<Rigidbody, float> _impulseDownPerRigidBody = new Dictionary<Rigidbody, float>();
    public Dictionary<Rigidbody, float> _impulseUpPerRigidBody = new Dictionary<Rigidbody, float>();

    float currentDeltaTime;
    float lastDeltaTime;

    public float moveScaleTimer;

    public MeshRenderer mesh;

    private void Awake()
    {
        forceToMass = 1f / Physics.gravity.magnitude;
        mesh = GetComponent<MeshRenderer>();
    }

    void UpdateWeight()
    {
        registeredRigidbodies = _impulseDownPerRigidBody.Count + _impulseUpPerRigidBody.Count;
        combinedForce = 0;

        foreach (var force in _impulseDownPerRigidBody.Values)
        {
            combinedForce += force;
        }

        foreach (var force in _impulseUpPerRigidBody.Values)
        {
            combinedForce += force;
        }

        combinedForce = Mathf.Round(combinedForce);

        calculatedMass = combinedForce * forceToMass;
    }

    private void FixedUpdate()
    {
        lastDeltaTime = currentDeltaTime;
        currentDeltaTime = Time.deltaTime;

        var currentVelocity = Vector3.zero;
        if (registeredRigidbodies > 0 && calculatedMass != 0)
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition,
                new Vector3(transform.localPosition.x,
                    Mathf.Clamp(-calculatedMass + transform.localPosition.y, -0.25f, 1.25f), transform.localPosition.z),
                ref currentVelocity, moveScaleTimer);
        else if(registeredRigidbodies > 0 && calculatedMass == 0)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition,
                new Vector3(transform.localPosition.x, 1, transform.localPosition.z), ref currentVelocity,
                moveScaleTimer);
        }

        // add calculated mass to transform y
        //transform.localPosition = new Vector3(transform.position.x, calculatedMass, transform.position.z);

        //Change emission color based on the weight
        if (combinedForce == 0 && registeredRigidbodies > 0 && transform.localPosition.y <= 1.1f && 
            transform.localPosition.y >= 0.9f)
        {
            mesh.material.SetColor("_EmissionColor", Color.green);
        }
        else
            mesh.material.SetColor("_EmissionColor", Color.red);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            if (_impulseDownPerRigidBody.ContainsKey(collision.rigidbody) &&
                collision.rigidbody.CompareTag("DownForce"))
                _impulseDownPerRigidBody[collision.rigidbody] = collision.impulse.y / lastDeltaTime;

            if (_impulseUpPerRigidBody.ContainsKey(collision.rigidbody) && collision.rigidbody.CompareTag("UpForce"))
                _impulseUpPerRigidBody[collision.rigidbody] = collision.impulse.y / lastDeltaTime;

            UpdateWeight();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            if (_impulseDownPerRigidBody.ContainsKey(collision.rigidbody) &&
                collision.rigidbody.CompareTag("DownForce"))
                _impulseDownPerRigidBody[collision.rigidbody] = collision.impulse.y / lastDeltaTime;

            if (_impulseUpPerRigidBody.ContainsKey(collision.rigidbody) && collision.rigidbody.CompareTag("UpForce"))
                _impulseUpPerRigidBody[collision.rigidbody] = collision.impulse.y / lastDeltaTime;

            UpdateWeight();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            _impulseDownPerRigidBody.Remove(collision.rigidbody);
            _impulseUpPerRigidBody.Remove(collision.rigidbody);
            UpdateWeight();
        }
    }
}