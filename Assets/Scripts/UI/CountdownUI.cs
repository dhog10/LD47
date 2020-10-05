using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownUI : MonoBehaviour
{
    private Text m_Text;

    void Start()
    {
        m_Text = this.GetComponent<Text>();
    }

    void Update()
    {
        var text = Countdown.Instance == null ? "" : Countdown.Instance.Time.ToString("00");
        m_Text.text = GameManager.Instance.IsGameOver ? "" : text;
    }
}
