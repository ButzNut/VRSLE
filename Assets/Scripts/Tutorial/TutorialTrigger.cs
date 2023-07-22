using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
   [SerializeField] private int audioPhase;
   [SerializeField] private bool isTriggered;
   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player") && !isTriggered)
      {
         TutorialManager.Instance.PlayAudio(audioPhase);
         this.isTriggered = true;
      }
   }
}
