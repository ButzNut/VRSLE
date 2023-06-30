using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShip : MonoBehaviour
{
    [HideInInspector] public Rigidbody rb;
    public float movementSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    public void MoveShipXZ(Vector3 direction)
    {
        direction = rb.rotation * direction;
        rb.AddForce(direction * movementSpeed);
    }
    
    public void MoveShipY(Vector3 direction)
    {
        rb.AddForce(direction * movementSpeed);
    }
}
