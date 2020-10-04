using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guage : MonoBehaviour
{
    public SteamPipe[] m_Pipes;

    public float Pressure {
        get {
            var numPipes = m_Pipes.Length;
            var numSealed = 0;

            foreach (var pipe in m_Pipes)
            {
                if (!pipe.Leaking)
                {
                    numSealed++;
                }
            }

            return numSealed / numPipes;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
