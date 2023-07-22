using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class DialChange : MonoBehaviour
{
    [Header("Dials and Text")]
    public XRKnob knob1;
    [SerializeField] int knob1Value;
    public XRKnob knob2;
    [SerializeField] int knob2Value;
    public XRKnob knob3;
    [SerializeField] int knob3Value;
    
    public TMP_Text equation1;
    public TMP_Text equation2;
    public TMP_Text equation3;
    
    [Header("Power crystals")]
    public GameObject powerCrystal1;
    public GameObject powerCrystal2;
    public GameObject powerCrystal3;
    
    [Header("Force Equation")]
    public List<ForceEquation> forceEquations = new List<ForceEquation>();
    public List<ForceEquation> shownEquations = new List<ForceEquation>();

    private void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            forceEquations.Add(new ForceEquation(0, UnityEngine.Random.Range(1, 7), UnityEngine.Random.Range(1, 7)));
            forceEquations[i].Force = forceEquations[i].CalculateForce();
        }
        
        for (int i = 0; i < 3; i++)
        {
            ForceEquation equation = forceEquations[UnityEngine.Random.Range(0, forceEquations.Count)];
            shownEquations.Add(equation);
            forceEquations.Remove(equation);
        }
    }

    private void Update()
    {
        knob1Value = (int) (knob1.value * 50);
        knob2Value = (int) (knob2.value * 50);
        knob3Value = (int) (knob3.value * 50);
        
        equation1.text = shownEquations[0].Mass + " * " + shownEquations[0].Acceleration + " = " + knob1Value;
        equation2.text = shownEquations[1].Mass + " * " + shownEquations[1].Acceleration + " = " + knob2Value;
        equation3.text = shownEquations[2].Mass + " * " + shownEquations[2].Acceleration + " = " + knob3Value;
    }


    public void CheckValues()
    {
        if (knob1Value == shownEquations[0].Force && knob2Value == shownEquations[1].Force && knob3Value == shownEquations[2].Force)
        {
            powerCrystal1.SetActive(true);
            powerCrystal2.SetActive(true);
            powerCrystal3.SetActive(true);
            GameManager.Instance.currentPhase = 3;
            GameManager.Instance.puzzle2Solved = true;
            GameManager.Instance.StartCoroutine(GameManager.Instance.Puzzle3Starter());
        }
        else
        {
            NewEquation();
        }
    
    }
    
    public void NewEquation()
    {
        for (int i = 0; i < 3; i++)
        {
            ForceEquation oldEquation = shownEquations[i];
            forceEquations.Add(oldEquation);
            shownEquations.Remove(oldEquation);

            ForceEquation equation = forceEquations[UnityEngine.Random.Range(0, forceEquations.Count)];
            shownEquations.Add(equation);
            forceEquations.Remove(equation);
        }
    }
    
    
    
}
[System.Serializable]
public class ForceEquation
{
    public int Force;
    public int Mass;
    public int Acceleration;
    
    public ForceEquation(int force, int mass, int acceleration)
    {
        Force = CalculateForce();
        Mass = mass;
        Acceleration = acceleration;
    }
    
    public int CalculateForce()
    {
        return Mass * Acceleration;
    }
}

