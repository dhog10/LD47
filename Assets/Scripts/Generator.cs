using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public static Generator Instance;

    public AudioSource m_TaskActivateSound;
    public AudioSource m_ExplosionSound;
    public AudioSource m_AlarmSound;
    public GameObject m_FireEffects;
    public TaskIndicator[] m_Tasks;

    private ParticleSystem[] m_ParticleFires;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        m_ParticleFires = m_FireEffects.GetComponentsInChildren<ParticleSystem>();

        foreach (var fire in m_ParticleFires)
        {
            fire.Stop();
        }
    }

    void Update()
    {
        var countdown = Countdown.Instance;

        if (countdown != null)
        {
            var severity = Mathf.Round((1f - (float)countdown.Time / (float)countdown.Duration) * m_ParticleFires.Length);

            for (var i = 0; i < severity; i++)
            {
                if (!m_ParticleFires[i].isPlaying)
                {
                    m_ParticleFires[i].Play();
                }
            }
        }
        
    }

    public void Explode()
    {
        m_ExplosionSound?.Play();
        TankCharacterController.Instance.Kill();
        GameManager.Instance.EndGame(false);
    }

    public void PlayTaskSound()
    {
        m_TaskActivateSound?.Play();
    }
}
