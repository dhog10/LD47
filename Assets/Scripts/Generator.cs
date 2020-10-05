using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public static Generator Instance;

    public AudioSource m_TaskActivateSound;
    public TaskIndicator[] m_Tasks;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlayTaskSound()
    {
        m_TaskActivateSound?.Play();
    }
}
