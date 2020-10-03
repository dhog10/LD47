using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskIndicator : Indicator
{
    public bool m_StartComplete;
    [Range(0f, 1f)]
    public float m_StartToggledChance = 0f;

    private bool m_Complete;

    public bool IsComplete
        => m_Complete;

    public override void Start()
    {
        base.Start();

        if (m_StartComplete) {
            m_Complete = true;
        }

        if (m_StartToggledChance > 0f && Random.Range(0f, 1f) <= m_StartToggledChance)
        {
            this.Toggle();
        }
    }

    public void Toggle()
    {
        if (m_Complete)
        {
            this.Uncomplete();
        }
        else
        {
            this.Complete();
        }
    }

    public void Complete()
    {
        m_Complete = true;
    }

    public void Uncomplete()
    {
        m_Complete = false;
    }

    public override bool IsOn()
    {
        return this.IsComplete;
    }
}
