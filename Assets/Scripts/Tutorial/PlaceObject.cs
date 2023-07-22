using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObject : MonoBehaviour
{
    public Transform moveWall;
    public Transform wallDestination;
    public bool isPlaced;

    private void Update()
    {
        if (isPlaced)
            moveWall.position =
                Vector3.Lerp(moveWall.position, wallDestination.transform.position, 1f * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ship") && !isPlaced)
        {
            isPlaced = true;
            TutorialManager.Instance.PlayAudio(3);
        }
    }
}