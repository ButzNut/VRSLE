using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class GravityPuzzle : MonoBehaviour
{
    
    public XRKnob knob1;
    public TMP_Text value1Text;
    [SerializeField] int knob1Value;
    
    // Update is called once per frame
    void Update()
    {
        knob1Value = Mathf.RoundToInt(knob1.value * 10);    
        value1Text.text = knob1Value.ToString();
    }
    
    public void CheckValues()
    {
        if (knob1Value == 3)
        {
            GameManager.Instance.puzzle4Solved = true;
        }
    }
    
}
