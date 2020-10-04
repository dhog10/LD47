using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamPipe : MonoBehaviour
{
    public AudioSource m_SteamSound;
    public ParticleSystem m_ParticleSystem;
    private bool m_Leaking = true;
    private float m_OriginalVolume;

    public bool Leaking
        => m_Leaking;

    void Start()
    {
        m_OriginalVolume = m_SteamSound.volume;
    }

    void Update()
    {
        if (this.Leaking != !m_ParticleSystem.isStopped)
        {
            if (this.Leaking)
            {
                m_ParticleSystem.Play();
            }
            else
            {
                m_ParticleSystem.Stop();
            }
        }

        var volume = this.Leaking ? m_OriginalVolume : 0f;
        m_SteamSound.volume += (volume - m_SteamSound.volume) * Time.deltaTime * 3f;
    }

    public void Seal(bool seal)
    {
        m_Leaking = seal;
    }
}
