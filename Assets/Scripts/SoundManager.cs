﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public MusicType m_DefaultMusicType;
    public MusicSource[] m_Music;

    private MusicType m_MusicType;
    private float m_LastChaseCheck;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            GameObject.DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            GameObject.DestroyImmediate(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        this.SetMusicType(m_DefaultMusicType);
    }

    private void Update()
    {
        foreach (var source in m_Music)
        {
            this.HandleAudioSource(source, m_MusicType == source.m_MusicType);
        }
    }

    public void SetMusicType(MusicType type)
    {
        m_MusicType = type;
    }

    private void HandleAudioSource(MusicSource source, bool enabled)
    {
        var target = enabled ? source.m_Volume : 0f;
        source.m_CurrentVolume += (target - source.m_CurrentVolume) * Time.deltaTime * 2f;

        if (Mathf.Abs(target - source.m_CurrentVolume) < 0.01f)
        {
            source.m_CurrentVolume = target;
        }

        if (source.m_AudioSource.isPlaying && !enabled && source.m_CurrentVolume == 0f)
        {
            source.m_AudioSource.Stop();
        }else if(!source.m_AudioSource.isPlaying && enabled)
        {
            source.m_AudioSource.Play();
        }

        source.m_AudioSource.volume = source.m_CurrentVolume;
    }

    public void ResetSounds()
    {
        foreach (var source in m_Music)
        {
            source.m_AudioSource?.Stop();
        }
    }
}

[System.Serializable]
public class MusicSource
{
    public MusicType m_MusicType;
    public float m_Volume;
    public AudioSource m_AudioSource;
    [HideInInspector]
    public float m_CurrentVolume;
}