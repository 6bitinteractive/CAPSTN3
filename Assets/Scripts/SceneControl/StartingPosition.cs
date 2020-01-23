using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPosition : MonoBehaviour
{
    [Tooltip("This serves to identify this particular starting position.")]
    [SerializeField] private SceneData sceneDataId;

    private static List<StartingPosition> allStartingPositions = new List<StartingPosition>();

    private void OnEnable()
    {
        allStartingPositions.Add(this);
    }

    private void OnDisable()
    {
        allStartingPositions.Remove(this);
    }

    public static Transform FindStartingPosition(string pointName)
    {
        for (int i = 0; i < allStartingPositions.Count; i++)
        {
            if (allStartingPositions[i].sceneDataId.StartingPointName == pointName)
                return allStartingPositions[i].transform;
        }

        return null;
    }
}

