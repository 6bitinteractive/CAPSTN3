using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]

public class SceneTransitionEffect : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    public bool IsInTransition { get; private set; }

    private CanvasGroup canvasGroup;
    private static SceneController sceneController;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    private void Start()
    {
        sceneController = sceneController ?? SingletonManager.GetInstance<SceneController>();
    }

    // NOTE: Current implementation is a simple fade
    public IEnumerator StartTransitionEffect(float finalAlpha)
    {
        IsInTransition = true;

        canvasGroup.blocksRaycasts = true;

        // Calculate how fast the CanvasGroup should fade based on its current alpha, its final alpha, and how long it has to change between the two
        float fadeSpeed = Mathf.Abs(canvasGroup.alpha - finalAlpha) / duration;

        // While the CanvasGroup hasn't reached the final alpha yet...
        while (!Mathf.Approximately(canvasGroup.alpha, finalAlpha))
        {
            // ... move the alpha towards its target alpha
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);

            // Enable input sooner (so that UX doesn't feel like there's a delay in reading the input)
            if (canvasGroup.alpha <= 0.7f)
                canvasGroup.blocksRaycasts = false;

            // Wait for a frame then continue
            yield return null;
        }

        IsInTransition = false;
    }
}
