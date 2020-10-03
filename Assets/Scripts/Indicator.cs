using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public Material m_OnMaterial;
    public Material m_OffMaterial;
    public Material m_DisabledMaterial;
    public MeshRenderer m_IndicatorMesh;

    public virtual void Start()
    {

    }

    void Update()
    {
        if (this.IsDisabled())
        {
            if (m_DisabledMaterial != null)
            {
                m_IndicatorMesh.sharedMaterial = m_DisabledMaterial;
            }
        }
        else
        {
            if (this.IsOn())
            {
                m_IndicatorMesh.sharedMaterial = m_OnMaterial;
            }
            else
            {
                m_IndicatorMesh.sharedMaterial = m_OffMaterial;
            }
        }
    }

    public virtual bool IsOn()
    {
        return false;
    }

    public virtual bool IsDisabled()
    {
        return false;
    }
}
