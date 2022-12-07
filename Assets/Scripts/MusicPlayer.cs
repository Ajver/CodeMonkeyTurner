using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    [SerializeField] private AudioClip musicClip; 
    
    [SerializeField] private bool playOnAwake = true;
    [SerializeField] private float initialDelay;

    [SerializeField] private bool loop;
    [SerializeField] private float loopDelay;
    
    private AudioSource audioSource;

    private bool isPlayingTimerRunning;
    private float playingTimer;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = musicClip;

        if (musicClip == null)
        {
            return;
        }

        if (playOnAwake)
        {
            PlayDelayed(initialDelay);
        }
    }

    private void Update()
    {
        if (!isPlayingTimerRunning)
        {
            return;
        }

        playingTimer -= Time.deltaTime;

        if (playingTimer <= 0f)
        {
            OnPlayingTimerEnded();
        }
    }

    private void OnPlayingTimerEnded()
    {
        isPlayingTimerRunning = false;

        if (loop)
        {
            PlayDelayed(loopDelay);
        }
    }
    
    private void PlayDelayed(float delay)
    {
        audioSource.PlayDelayed(delay);
        
        playingTimer = musicClip.length + delay;
        isPlayingTimerRunning = true;
    }
    
}
