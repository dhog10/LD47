using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holdable : MonoBehaviour
{
    public float m_UseDistance = 1.5f;

    private Rigidbody m_Rigidbody;
    private Collider[] m_Colliders;

    public float UseDistance
        => m_UseDistance;

    void Start()
    {
        m_Rigidbody = this.GetComponentInChildren<Rigidbody>();
        m_Colliders = this.GetComponentsInChildren<Collider>();
    }

    public void Freeze()
    {
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        foreach (var collider in m_Colliders)
        {
            collider.enabled = false;
        }
    }

    public void Unfreeze()
    {
        m_Rigidbody.constraints = RigidbodyConstraints.None;

        foreach (var collider in m_Colliders)
        {
            collider.enabled = true;
        }
    }
}
