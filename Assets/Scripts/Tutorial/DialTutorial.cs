using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class DialTutorial : MonoBehaviour
{
    public XRKnob knob1;
    public TMP_Text value1Text;
    public GameObject platform;
    bool isPlaying = false;
    // Update is called once per frame
    void Update()
    {
        value1Text.text = "Dial value: " + knob1.value.ToString();

        if (knob1.value == 1 && !isPlaying)
        {
            platform.SetActive(true);
            TutorialManager.Instance.PlayAudio(7);
            isPlaying = true;
        }
    }
}
