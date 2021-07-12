using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSelection : MonoBehaviour
{
   GameManager gameManager;
   private void Awake()
   {
      // initialize variables
      
      gameManager = FindObjectOfType<GameManager>();

   }
   public void SelectAndGate()
   {
      Debug.Log($"And clicked");
      gameManager.SpawnGate(GateType.And);
   }
   
   public void SelectOrGate()
   {
      Debug.Log($"Or clicked");
      gameManager.SpawnGate(GateType.Or);
   }
   public void SelectNorGate()
   {
      Debug.Log($"Nor clicked");
      gameManager.SpawnGate(GateType.Nor);
   }
   public void SelectXorGate()
   {
      Debug.Log($"Xor clicked");
      gameManager.SpawnGate(GateType.Xor);
   }
   public void SelectXNorGate()
   {
      Debug.Log($"XNor clicked");
      gameManager.SpawnGate(GateType.XNor);
   }
   public void SelectNandGate()
   {
      Debug.Log($"XNand clicked");
      gameManager.SpawnGate(GateType.Nand);
   }
   public void SelectNotGate()
   {
      Debug.Log($"NotGate gate clicked");
      gameManager.SpawnGate(GateType.Not);
   }
   
   
  
}
