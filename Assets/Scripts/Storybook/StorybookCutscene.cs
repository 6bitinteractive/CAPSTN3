using Meowfia.WanderDog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StorybookCutscene : MonoBehaviour
{
    [Header("Storybook")]
    [SerializeField] private StorybookImageSet[] imageSets;
    [SerializeField] private float cutsceneDuration = 2f;
    [SerializeField] private Image storybookImage;

    [Header("Fade Panel")]
    [SerializeField] private CanvasGroup fadePanelCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private PlayerControlScheme controlScheme;
    private PlayerInput playerInput;
    private SceneLoader sceneLoader;
    private AudioSource audioSource;

    private int cutsceneIndex;
    private int currentSpriteIndex = -1;
    private bool isFading;
    private bool End => currentSpriteIndex == imageSets[cutsceneIndex].storybookImage.Length - 1;

    private void Start()
    {
        controlScheme = new PlayerControlScheme();
        controlScheme.DialogueInteraction.Enable();

        playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap("DialogueInteraction");

        controlScheme.DialogueInteraction.Confirm.performed += context => SkipFade();

        sceneLoader = GetComponent<SceneLoader>();
        audioSource = GetComponent<AudioSource>();

        DayProgression dayProgression = SingletonManager.GetInstance<GameManager>().DayProgression;
        cutsceneIndex = dayProgression.CurrentDayIndex - 1;

        StartCoroutine(FadeToNextImage());

    }

    private void SkipFade()
    {
        if (End)
        {
            sceneLoader.LoadScene(imageSets[cutsceneIndex].sceneToLoadNext);
            return;
        }

        if (isFading)
        {
            StopAllCoroutines();
            fadePanelCanvasGroup.alpha = 0f;
            fadePanelCanvasGroup.blocksRaycasts = false;
        }

        SwitchToNextImage();
    }

    private IEnumerator FadeToNextImage()
    {
        while (!End)
        {
            yield return StartCoroutine(Fade(1f)); // Fade to black
            SwitchToNextImage();

            //Play sfx
            if (audioSource.clip != null)
            {
                audioSource.Play();
                Debug.Log("Play sfx");
            }

            yield return StartCoroutine(Fade(0f)); // Show image
            Debug.Log(currentSpriteIndex);
            yield return new WaitForSeconds(cutsceneDuration);
            yield return null;
        }

        sceneLoader.LoadScene(imageSets[cutsceneIndex].sceneToLoadNext);
    }

    private IEnumerator Fade(float finalAlpha)
    {
        isFading = true;

        fadePanelCanvasGroup.blocksRaycasts = true;

        // Calculate how fast the CanvasGroup should fade based on its current alpha, its final alpha, and how long it has to change between the two
        float fadeSpeed = Mathf.Abs(fadePanelCanvasGroup.alpha - finalAlpha) / fadeDuration;

        // While the CanvasGroup hasn't reached the final alpha yet...
        while (!Mathf.Approximately(fadePanelCanvasGroup.alpha, finalAlpha))
        {
            // ... move the alpha towards its target alpha
            fadePanelCanvasGroup.alpha = Mathf.MoveTowards(fadePanelCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);

            // Enable input sooner (so that UX doesn't feel like there's a delay in reading the input)
            if (fadePanelCanvasGroup.alpha <= 0.7f)
                fadePanelCanvasGroup.blocksRaycasts = false;

            // Wait for a frame then continue
            yield return null;
        }

        isFading = false;
    }

    private void SwitchToNextImage()
    {
        currentSpriteIndex++;
        storybookImage.sprite = imageSets[cutsceneIndex].storybookImage[currentSpriteIndex].sprite;
        audioSource.clip = imageSets[cutsceneIndex].storybookImage[currentSpriteIndex].audioClip;
    }
}

[System.Serializable]
public class StorybookImageSet
{
    public int day;
    public SceneData sceneToLoadNext;
    public StorybookImage[] storybookImage;

    [System.Serializable]
    public class StorybookImage
    {
        public Sprite sprite;

        [Tooltip("Optional sfx to play when image is displayed")]
        public AudioClip audioClip;
    }
}
