using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Usable : MonoBehaviour
{
    [Header("Usable")]
    public float m_UseDistance = 2f;
    public bool m_Toggle = false;
    public bool m_CanDeactivate = true;
    public bool m_StartDisabled;
    public float m_DeactivateTime = 0.5f;
    public AudioSource m_ActivateSound;
    public AudioSource m_DeactivateSound;
    public UnityEvent m_ActivateEvent;
    public UnityEvent m_DeactivateEvent;

    private float m_ActiveTime;
    private bool m_Active;
    private bool m_Disabled;

    public bool IsActive
        => m_Active;

    public bool IsToggle
        => m_Toggle;

    public float UseDistance
        => m_UseDistance;

    public virtual void Start()
    {
        if (m_StartDisabled)
        {
            m_Disabled = true;
        }
    }

    public virtual void Update()
    {
        if (!this.IsToggle && this.IsActive && Time.time - m_ActiveTime >= m_DeactivateTime)
        {
            this.Deactivate();
        }
    }

    public bool Toggle()
    {
        if (this.IsToggle)
        {
            if (this.IsActive)
            {
                if (!m_CanDeactivate)
                {
                    return false;
                }

                this.Deactivate();
            }
            else
            {
                this.Activate();
            }

            return true;
        }
        else if (!this.IsActive)
        {
            this.Activate();
            return true;
        }

        return false;
    }

    public virtual void Activate()
    {
        m_Active = true;
        m_ActiveTime = Time.time;

        if (m_ActivateSound != null)
        {
            m_ActivateSound.Stop();
            m_ActivateSound.Play();
        }

        m_ActivateEvent?.Invoke();
    }

    public virtual void Deactivate()
    {
        m_Active = false;

        if (m_DeactivateSound != null)
        {
            m_DeactivateSound.Stop();
            m_DeactivateSound.Play();
        }

        m_DeactivateEvent?.Invoke();
    }

    public void Disable(bool disabled) {
        m_Disabled = disabled;
    }

    public virtual bool IsDisabled()
    {
        return m_Disabled;
    }
}
