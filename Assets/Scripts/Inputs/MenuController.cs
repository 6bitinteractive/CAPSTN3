using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// This script's main purpose is to handle Menu navigation for the Gamepad/Controller
public class MenuController : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject firstSelectedGameObject;

    void Start()
    {
        eventSystem.SetSelectedGameObject(firstSelectedGameObject, null);
    }
}
