using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidPool : MonoBehaviour
{
    public GameObject m_Plane;
    public float m_DrainDepth = 2f;
    public float m_DrainSpeed = 5f;
    public AudioSource m_DrainSound;

    private bool m_Drained;
    private Vector3 m_OriginalPlanePos;

    public bool IsDrained
        => m_Drained;

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

        if (m_DrainSound != null)
        {
            m_DrainSound.Stop();
            m_DrainSound.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == null)
        {
            return;
        }

        var player = other.attachedRigidbody.GetComponentsInChildren<TankCharacterController>();
        if (player != null)
        {
            // TODO: End game
        }
    }
}
