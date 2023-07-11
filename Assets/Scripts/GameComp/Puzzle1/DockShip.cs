using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockShip : MonoBehaviour
{
    public Transform spaceDock, spaceShip;
    public bool docking = false;
    public MoveShip moveShip;


    private void FixedUpdate()
    {
        if (docking)
        {
            //Move the ship according to the child of the ship and stop when it reaches the dock
            spaceShip.position = Vector3.MoveTowards(spaceShip.position, spaceDock.position, 0.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ship"))
        {
            //stop the rigidbody movement
            moveShip.rb.velocity = Vector3.zero;
            docking = true;
        }
    }
}
