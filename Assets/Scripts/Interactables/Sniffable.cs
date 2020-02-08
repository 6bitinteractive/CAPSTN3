using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniffable : MonoBehaviour
{
    //[SerializeField] private GuidReference crossSceneReferenceQuestEvent;

    //private QuestEvent questEvent;
    private Sniff sniff;
    private void Start()
    {
        //questEvent = crossSceneReferenceQuestEvent.gameObject.GetComponent<QuestEvent>();
        //questEvent.OnActive.gameEvent.AddListener(SetCurrentTarget);
        //questEvent.OnDone.gameEvent.AddListener(RemoveCurrentTargetSniffable);
    }

    public void SetCurrentTarget()
    {
        sniff = FindObjectOfType<Sniff>();
        sniff.CurrentDestination = gameObject.transform;
    }

    public void RemoveCurrentTargetSniffable()
    {
        sniff.CurrentDestination = null;
    }
}