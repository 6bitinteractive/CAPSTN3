using Meowfia.WanderDog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneLoader))]

public class DayProgressionController : MonoBehaviour
{
    private static SceneData storyBookScene;
    private static DayProgression dayProgression;
    private SceneLoader sceneLoader;

    private void Start()
    {
        storyBookScene = storyBookScene ?? Resources.Load<SceneData>("SceneData/Storybook");
        dayProgression = dayProgression ?? SingletonManager.GetInstance<GameManager>().DayProgression;
        sceneLoader = GetComponent<SceneLoader>();
    }

    public void BeginDay()
    {
        dayProgression.BeginDay(dayProgression.CurrentDayIndex);
    }

    public void EndDay()
    {
        dayProgression.EndDay();
        Debug.Log(storyBookScene);
        sceneLoader.LoadScene(storyBookScene);
    }
}
