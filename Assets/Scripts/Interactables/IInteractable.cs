using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void DisplayInteractability(); // Handles highlighting objects when nearby or displaying an icon to display interactability of an object
    void Interact(Interactor source, IInteractable target); // Handles interaction
}