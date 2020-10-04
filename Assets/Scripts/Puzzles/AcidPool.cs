using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidPool : MonoBehaviour
{
    public GameObject m_Plane;
    public float m_DrainDepth = 2f;
    public float m_DrainSpeed = 5f;
    public AudioSource m_GunkSound;
    public AudioSource m_DrainSound;

    private bool m_Drained;
    private Vector3 m_OriginalPlanePos;
    private float m_DrainedTime;

    public bool IsDrained
        => m_Drained;

    public bool IsFullyDrained
    {
        get
        {
            if (!this.IsDrained)
            {
                return false;
            }

            Debug.Log((m_DrainDepth / m_DrainSpeed) * 0.75f);

            var diff = Time.time - m_DrainedTime;
            return diff >= (m_DrainDepth / m_DrainSpeed) * 0.5f;
        }
    }

    private void Start()
    {
        m_OriginalPlanePos = m_Plane.transform.localPosition;
    }

    void Update()
    {
        var pos = this.IsDrained ? m_OriginalPlanePos - new Vector3(0f, m_DrainDepth, 0f) : m_OriginalPlanePos;
        m_Plane.transform.localPosition += (pos - m_Plane.transform.localPosition) * Time.deltaTime * m_DrainSpeed;
    }

    public void Drain()
    {
        m_Drained = true;
        m_DrainedTime = Time.time;

        if (m_DrainSound != null)
        {
            m_DrainSound.Stop();
            m_DrainSound.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.IsFullyDrained)
        {
            return;
        }

        if (other.attachedRigidbody == null)
        {
            return;
        }

        var player = other.attachedRigidbody.GetComponentsInChildren<TankCharacterController>();
        if (player != null)
        {
            GameManager.Instance.EndGame(false);
        }
    }
}
