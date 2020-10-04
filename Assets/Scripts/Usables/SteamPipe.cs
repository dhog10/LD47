using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamPipe : MonoBehaviour
{
    private bool m_Leaking = true;

    public bool Leaking
        => m_Leaking;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Seal()
    {
        m_Leaking = false;
    }
}
