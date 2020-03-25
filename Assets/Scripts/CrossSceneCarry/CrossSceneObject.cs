using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Biteable))]

public class CrossSceneObject : MonoBehaviour
{
    [Tooltip("Scene where crossScene deliverables are stored; by default it is the Persistent scene")]
    [SerializeField] private SceneData persistentDeliverable;

    public bool IsCarried { get; set; }

    private Biteable biteable;

    private void Awake()
    {
        biteable = GetComponent<Biteable>();
    }

    private void OnEnable()
    {
        biteable.OnBite.AddListener(OnCarried);
        biteable.OnRelease.AddListener(OnReleased);
    }

    private void OnDisable()
    {
        biteable.OnBite.RemoveListener(OnCarried);
        biteable.OnRelease.RemoveListener(OnReleased);
    }

    private void OnCarried()
    {
        IsCarried = true;
    }

    private void OnReleased()
    {
        IsCarried = false;
        MoveToPersistentScene();
    }

    public void MoveToPersistentScene()
    {
        gameObject.transform.SetParent(null);
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(persistentDeliverable.SceneName));
    }

    public void MoveToCurrentActiveScene()
    {
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(SceneManager.GetActiveScene().name));
    }
}
