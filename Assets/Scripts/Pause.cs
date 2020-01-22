using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject firstSelectedGameObject;

    public void PauseGame()
    {       
        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
        eventSystem.SetSelectedGameObject(firstSelectedGameObject, null); // This is to allow controller navigation
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
        eventSystem.SetSelectedGameObject(null, null);
    }
}