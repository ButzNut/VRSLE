using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScaleDisplay : MonoBehaviour
{
    public ForceCalculator forceCalculator;
    public TMP_Text forceText;

    // Update is called once per frame
    void Update()
    {
        if (forceCalculator.rbUp.Count > 0 && forceCalculator.rbDown.Count > 0)
        {
            forceText.text = forceCalculator.totalForce.ToString("F2") + " N";
        }
        else if (forceCalculator.rbUp.Count > 0 || forceCalculator.rbDown.Count > 0)
        {
            forceText.text = forceCalculator.totalForce.ToString("F2") + " N";
        }
        else if(forceCalculator.rbUp.Count == 0 && forceCalculator.rbDown.Count == 0)
        {
            forceText.text = "Scale empty";
        }
    }
}