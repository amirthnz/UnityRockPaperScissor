using System.Collections.Generic;
using UnityEngine;

// Serializable class to hold sound data
[System.Serializable]
public class Sound
{
    public string name;         // Name of the sound, used to identify it
    public AudioClip clip;      // The audio clip to be played
    [HideInInspector] public AudioSource source; // AudioSource component for playing the sound
}

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds;  // List of sounds that can be played

    public static AudioManager Instance; // Singleton instance of AudioManager

    private void Awake()
    {
        // Implement the singleton pattern
        if (Instance == null)
        {
            Instance = this; // Assign this instance to the singleton
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances to maintain a single instance
        }
    }

    private void Start()
    {
        // Create an AudioSource for each sound in the list
        foreach (var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>(); // Add an AudioSource component
            sound.source.clip = sound.clip; // Assign the AudioClip to the AudioSource
            sound.source.playOnAwake = false; // Prevent the sound from playing automatically on awake
            sound.source.volume = 0.7f; // Set AudioSource initial volume
        }
    }

    // Method to play a sound by name
    public void Play(string soundName)
    {
        // Find the sound in the list by name
        Sound sound = sounds.Find(s => s.name == soundName);
        if (sound != null)
        {
            sound.source.Play(); // Play the sound if found
        }
        else
        {
            // Log a warning if the sound is not found
            Debug.LogWarning($"Sound '{soundName}' not found!");
        }
    }
}