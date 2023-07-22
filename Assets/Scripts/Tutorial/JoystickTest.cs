using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class JoystickTest : MonoBehaviour
{
    public XRJoystick joystick;
    public MeshRenderer topMesh;
    public MeshRenderer bottomMesh;
    public MeshRenderer leftMesh;
    public MeshRenderer rightMesh;

    public bool top;
    public bool bottom;
    public bool left;
    public bool right;

    bool isPlaying = false;
    
    public GameObject platform;

    // Update is called once per frame
    void Update()
    {
        if (joystick.value.y > 0.8f)
            top = true;

        if (joystick.value.y < -0.8f)
            bottom = true;

        if (joystick.value.x > 0.8f)
            right = true;

        if (joystick.value.x < -0.8f)
            left = true;

        if (top)
            topMesh.materials[0].SetColor("_EmissionColor", Color.green * 50);


        if (bottom)
            bottomMesh.materials[0].SetColor("_EmissionColor", Color.green * 50);

        if (left)
            leftMesh.materials[0].SetColor("_EmissionColor", Color.green * 50);

        if (right)
            rightMesh.materials[0].SetColor("_EmissionColor", Color.green * 50);

        if (top && bottom && left && right && !isPlaying)
        {
            TutorialManager.Instance.PlayAudio(5);
            platform.SetActive(true);
            isPlaying = true;
        }
    }
}