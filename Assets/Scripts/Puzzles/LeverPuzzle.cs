using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeverPuzzle : MonoBehaviour
{
    public AudioSource m_WrongSound;
    public Button[] m_Levers;
    public Indicator[] m_Indicators;

    public UnityEvent m_OnCompleteEvent;

    private int m_LeverIndex;

    void Start()
    {
        m_LeverIndex = Random.Range(0, m_Levers.Length);
        m_Indicators[m_LeverIndex].ActivateIndicator(true);

        var c = 0;
        foreach (var lever in m_Levers)
        {
            var index = c++;
            lever.m_ActivateEvent.AddListener(() =>
            {
                if (index == m_LeverIndex)
                {
                    m_OnCompleteEvent?.Invoke();
                }
                else
                {
                    m_WrongSound?.Play();
                    Countdown.Instance.TakeTime(10);
                }
            });
        }
    }
}
