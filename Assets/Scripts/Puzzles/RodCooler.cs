using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RodCooler : MonoBehaviour
{
    public Button m_Door;
    public Light m_Light;
    public Color m_OnColor;
    public Color m_OnWithRodColor;
    public Color m_OffColor;
    public UnityEvent m_ActivatedEvent;
    public UnityEvent m_DeactivatedEvent;

    private bool m_WasOn;
    private bool m_On;
    private Rod m_Rod;

    public bool IsOn
        => m_On;

    public bool HasRod
        => m_Rod != null;

    void Start()
    {
        
    }

    void Update()
    {
        m_Light.color = m_On ? (this.HasRod ? m_OnWithRodColor : m_OnColor) : m_OffColor;

        if (m_Door.IsActive)
        {
            this.Deactivate();
        }
        else
        {
            this.Activate();
        }

        var powered = this.IsOn && this.HasRod;
        if (powered != m_WasOn)
        {
            m_WasOn = powered;

            if (this.IsOn)
            {
                this.m_ActivatedEvent?.Invoke();
            }
            else
            {
                this.m_DeactivatedEvent?.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == null)
        {
            return;
        }

        var rod = other.attachedRigidbody.GetComponentInChildren<Rod>();

        if (rod != null)
        {
            m_Rod = rod;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody == null)
        {
            return;
        }

        var rod = other.attachedRigidbody.GetComponentInChildren<Rod>();

        if (rod != null && rod == m_Rod)
        {
            m_Rod = null;
        }
    }

    public void Activate()
    {
        if (m_On)
        {
            return;
        }

        m_On = true;
    }

    public void Deactivate()
    {
        if (!m_On)
        {
            return;
        }

        m_On = false;
    }
}
