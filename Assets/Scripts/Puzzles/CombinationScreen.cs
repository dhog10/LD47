using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombinationScreen : MonoBehaviour
{
    public MeshFilter m_MeshFilter;

    private TextMeshProUGUI m_Text;

    void Start()
    {
        m_Text = this.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetIndex(int index)
    {
        if (m_Text == null)
        {
            m_Text = this.GetComponentInChildren<TextMeshProUGUI>();

            if (m_Text == null)
            {
                return;
            }
        }

        m_Text.text = index.ToString();
    }
}
