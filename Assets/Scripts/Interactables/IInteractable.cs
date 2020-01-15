using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    Vector3 position { get; }
    void DisplayInteractability(); // Handles highlighting objects when nearby or displaying an icon to display interactability of an object
    void Interact(); // Handles interaction
}