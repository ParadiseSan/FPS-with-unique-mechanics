using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatEventManager : MonoBehaviour
{
    public AudioClip audioClip;
    public float[] beatTimings; // Time of each beat in seconds
    public float beatDetectionTolerance = 0.1f; // Adjust as needed

    private AudioSource audioSource;
    private bool[] beatTriggered;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();

        // Initialize array to keep track of triggered beats
        beatTriggered = new bool[beatTimings.Length];
    }

    void Update()
    {
        // Check for beat events
        CheckForBeatEvent();
    }

    void CheckForBeatEvent()
    {
        // Get the elapsed time since the audio started playing
        float elapsedTime = audioSource.time;

        // Iterate through each beat timing
        for (int i = 0; i < beatTimings.Length; i++)
        {
            // Check if the current beat has not been triggered and the elapsed time is within tolerance
            if (!beatTriggered[i] && Mathf.Abs(elapsedTime - beatTimings[i]) < beatDetectionTolerance)
            {
                // Trigger event corresponding to the current beat
                TriggerEvent(i);
                // Mark the beat as triggered
                beatTriggered[i] = true;
            }
        }
    }

    void TriggerEvent(int beatIndex)
    {
        // Trigger event for the specified beat index
        Debug.Log("Beat Event Triggered at Time: " + audioSource.time + ", Beat Index: " + beatIndex);
        // Add your event logic here
    }
}
