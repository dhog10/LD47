#pragma warning disable 0649

using System.Collections;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public static Countdown Instance;

    [SerializeField] private string m_CountdownPrefix;
    [SerializeField] private int m_CountdownDuration = 60;
    [SerializeField] private TextMeshPro m_Text;
    private int m_CurrentTime;

    public string Text
        => m_Text.text;

    public int Duration
        => m_CountdownDuration;

    public int Time
        => m_CurrentTime;

    private void Start()
    {
        Instance = this;

        StartCoroutine(this.CountdownRoutine());

        if (m_CountdownPrefix.Length > 0)
        {
            m_CountdownPrefix = m_CountdownPrefix + "\n";
        }
    }

    private void OnEnable()
    {
        Instance = this;
    }

    public void TakeTime(int time)
    {
        m_CurrentTime = Mathf.Max(0, m_CurrentTime - time);
    }

    private IEnumerator CountdownRoutine()
    {
        m_CurrentTime = m_CountdownDuration;

        while (m_CurrentTime > 0)
        {
            m_Text?.SetText(m_CountdownPrefix + m_CurrentTime.ToString("00"));
            yield return new WaitForSeconds(1f);
            m_CurrentTime--;
        }
        m_Text?.SetText(m_CountdownPrefix + m_CurrentTime.ToString("00"));
    }

    public void AddToCountdown(int countdownIncrease)
        => m_CurrentTime += countdownIncrease;

    public void RemoveFromCountdown(int countdownDecrease)
        => m_CurrentTime -= countdownDecrease;
}

#pragma warning restore 0649
