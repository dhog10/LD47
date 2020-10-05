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
    private bool m_Exploded;

    private void Awake()
    {
        Instance = this;
        m_Exploded = false;
    }

    void Start()
    {
        m_ParticleFires = m_FireEffects.GetComponentsInChildren<ParticleSystem>();

        foreach (var fire in m_ParticleFires)
        {
            fire.Stop();
        }

        m_Exploded = false;
    }

    void Update()
    {
        var countdown = Countdown.Instance;

        if (countdown != null)
        {
            var severity = Mathf.Round((1f - (float)countdown.Time / (float)countdown.Duration) * m_ParticleFires.Length);

            if (!m_AlarmSound.isPlaying && severity / m_ParticleFires.Length > 0.7f)
            {
                m_AlarmSound.Play();
            }

            for (var i = 0; i < severity; i++)
            {
                if (!m_ParticleFires[i].isPlaying)
                {
                    m_ParticleFires[i].Play();
                }
            }

            if (!m_Exploded && countdown.Time <= 0)
            {
                m_Exploded = true;
                this.Explode();
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
