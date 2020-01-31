using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDisplayManager : Singleton<DialogueDisplayManager>
{
    [HideInInspector] public List<DialogueDisplay> dialogueDisplays = new List<DialogueDisplay>();

    public DialogueDisplay GetDialogueDisplay(DialogueSpeaker dialogueSpeaker)
    {
        return dialogueDisplays.Find(x => x.dialogueSpeaker == dialogueSpeaker);
    }
}
