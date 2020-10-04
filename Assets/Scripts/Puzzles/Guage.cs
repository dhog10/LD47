using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guage : MonoBehaviour
{
    public GameObject m_DialObject;
    public Vector3 m_DialAngle;
    public SteamPipe[] m_Pipes;

    private Vector3 m_OriginalDialAngle;

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

    private void Start()
    {
        m_OriginalDialAngle = m_DialObject.transform.localRotation.eulerAngles;
    }

    void Update()
    {
        var pressure = this.Pressure;

        var newEuler = m_OriginalDialAngle + m_DialAngle * pressure;
        var newQ = Quaternion.RotateTowards(m_DialObject.transform.localRotation, Quaternion.Euler(newEuler.x, newEuler.y, newEuler.z), Time.deltaTime * 100f);

        m_DialObject.transform.localRotation = newQ;
    }
}
