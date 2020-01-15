using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bite : MonoBehaviour
{
    public void BiteEvent(Biteable target)
    {
        target.Interact();
    }
}