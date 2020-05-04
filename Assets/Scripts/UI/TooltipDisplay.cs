using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]

public class TooltipDisplay : MonoBehaviour
{
    [SerializeField] private AudioClip TooltipAppearSFX;

    public TextMeshProUGUI tooltipText;

    private AudioSource audioSource;
    private static EventManager eventManager;
    private static QuestCollection questCollection;

    //public Animator animator;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
        questCollection = questCollection ?? SingletonManager.GetInstance<QuestManager>().CurrentQuestCollection;

        eventManager.Subscribe<GameQuestEvent, QuestEvent>(UpdateTooltipDisplay);

        tooltipText.text = questCollection.CurrentQuestEvent.DisplayName.ToString();
        //animator.SetBool("IsOpen", false);
    }

    private void OnDestroy()
    {
        eventManager.Unsubscribe<GameQuestEvent, QuestEvent>(UpdateTooltipDisplay);
    }

    private void UpdateTooltipDisplay(QuestEvent questEvent)
    {
        if (questCollection == null || questCollection.CurrentQuestEvent == null)
        {
            return;
        }

        //animator.SetBool("IsOpen", true);
        tooltipText.text = questCollection.CurrentQuestEvent.DisplayName.ToString();

        audioSource.clip = TooltipAppearSFX;
        audioSource.Play();
    }
}
