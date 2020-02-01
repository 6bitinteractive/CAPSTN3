using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniffable : MonoBehaviour
{
    [SerializeField] private GuidReference crossSceneReferenceQuestEvent;

    private QuestEvent questEvent;
    private Sniff sniff;
    void Awake()
    {
        sniff = FindObjectOfType<Sniff>();
        questEvent = crossSceneReferenceQuestEvent.gameObject.GetComponent<QuestEvent>();
        questEvent.OnActive.AddListener(SetCurrentTarget);
        questEvent.OnDone.AddListener(RemoveCurrentTargetSniffable);
    }

    public void SetCurrentTarget(QuestEvent questEvent)
    {
        sniff.CurrentDestination = gameObject.transform;
    }

    public void RemoveCurrentTargetSniffable(QuestEvent questEvent)
    {
        sniff.CurrentDestination = null;
    }
}