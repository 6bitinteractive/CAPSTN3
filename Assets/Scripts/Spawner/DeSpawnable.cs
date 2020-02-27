using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeSpawnable : MonoBehaviour
{
  public void Despawn()
   {
        gameObject.SetActive(false);
   }
}