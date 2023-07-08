using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShip : MonoBehaviour
{
    [HideInInspector] public Rigidbody rb;
    public float movementSpeed;
    [SerializeField]
    private ParticleSystem _thruster;
    private ParticleSystem.MainModule _thrusterMain;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        _thrusterMain = _thruster.main;
    }

    public void MoveShipXZ(Vector3 direction)
    {
        direction = rb.rotation * direction;
        rb.AddForce(direction * movementSpeed);
        _thrusterMain.startSpeed = -direction.magnitude * 2;
        _thrusterMain.startSize = direction.magnitude;
    }
}
