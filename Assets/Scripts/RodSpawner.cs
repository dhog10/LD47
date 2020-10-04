using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodSpawner : MonoBehaviour
{
    public GameObject m_RodPrefab;

    private GameObject m_RodObject;

    void Update()
    {
        if (m_RodObject == null)
        {
            this.SpawnRod();
        }
    }

    private void SpawnRod()
    {
        var rod = GameObject.Instantiate(m_RodPrefab);
        rod.transform.position = transform.position;
        m_RodObject = rod;
    }
}
