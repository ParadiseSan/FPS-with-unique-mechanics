using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip songClip;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
            
    }

    public void PlaySong()
    {
        audioSource.clip = songClip;
        audioSource.Play();
        
        Debug.Log("Audio Started");
    }
}
