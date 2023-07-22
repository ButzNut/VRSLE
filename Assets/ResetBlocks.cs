using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBlocks : MonoBehaviour
{
   public GameObject[] blocks;
   public Transform[] blockPositions;

    public void Reset()
   {
      if(GameManager.Instance.puzzle3Solved)
         return;
      for (int i = 0; i < blocks.Length; i++)
      {
         blocks[i].transform.position = blockPositions[i].position;
         blocks[i].transform.rotation = blockPositions[i].rotation;
      }
   }
}
