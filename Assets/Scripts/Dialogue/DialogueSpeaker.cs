using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueSpeaker : ScriptableObject
{
    [SerializeField] private Sprite image;
    [SerializeField] private string speakerName;
    [SerializeField] private string speakerDescription;

    public Sprite Image => image;
    public string DisplayName => speakerName;
    public string Description => speakerDescription;
}
