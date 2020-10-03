using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskIndicator : MonoBehaviour
{
    public Material m_CompleteMaterial;
    public Material m_IncompleteMaterial;
    public MeshRenderer m_IndicatorMesh;

    void Start()
    {
        
    }

    void Update()
    {
        if (this.IsComplete())
        {
            m_IndicatorMesh.sharedMaterial = m_CompleteMaterial;
        }
        else
        {
            m_IndicatorMesh.sharedMaterial = m_IncompleteMaterial;
        }
    }

    public bool IsComplete()
    {
        return false;
    }
}
