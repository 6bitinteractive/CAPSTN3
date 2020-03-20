using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Add this component to objects with the AudioSource component containing the BGM or Ambient clips.
// Make sure to set what type of audio it is

[RequireComponent(typeof(AudioSource))]

public class AudioSwitcher : MonoBehaviour
{
    [SerializeField]
    [TextArea(3, 5)]
    private string note = "Some scenes do not have ambient sound so this is probably " +
        "attached to some object with an empty AudioSource component; simply ignore this script if such is the case." +
        " Please do not remove.";

    [SerializeField] private AudioType audioType;
    private AudioSource audioSource;
    private static AudioManager audioManager;

    private void Awake()
    {
        audioManager = new PersistentAudioManager(); //GameObject.FindGameObjectWithTag("AudioManager").GetComponent<PersistentAudioManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Turn off for now; this is more of a workaround since audio clips are already set up in most scenes
        audioSource.enabled = false;

        // Check
        AudioClip currentPlayingClip = audioManager.AudioSource(audioType).clip;
        if (audioSource.clip != currentPlayingClip || currentPlayingClip == null)
        {
            AudioSetting audioSetting = new AudioSetting(audioSource.clip, audioSource.volume);
            audioManager.SwitchAudio(audioSetting, audioType);
        }
    }
}

public enum AudioType
{
    BGM,
    Ambient
}

public struct AudioSetting
{
    public AudioClip clip;
    public float volume;

    public AudioSetting(AudioClip clip, float volume)
    {
        this.clip = clip;
        this.volume = volume;
    }
}
