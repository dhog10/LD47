﻿using System.Collections;
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
        var text = Countdown.Instance == null ? "" : Countdown.Instance.Time.ToString();
        m_Text.text = text;
    }
}
