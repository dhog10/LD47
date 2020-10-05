using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWin : MonoBehaviour
{
    public static GameWin Instance;

    public float m_LetterInterval = 0.9f;
    public float m_ResetDelay = 1f;
    public Text m_Text;

    private Image m_Panel;
    private Color m_PanelColor;
    private string m_OriginalText;
    private bool m_Enabled;
    private float m_LastLetter;
    private int m_LetterIndex;
    private float m_Alpha;
    private float m_ResetTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            GameObject.DestroyImmediate(gameObject);
            return;
        }

        Instance = this;

    }
    private void OnEnable()
    {
        if (Instance != null && Instance != this)
        {
            GameObject.DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        m_Panel = this.GetComponent<Image>();
        m_PanelColor = m_Panel.color;
        m_Panel.color = new Color(m_PanelColor.r, m_PanelColor.g, m_PanelColor.b, 0f);

        m_OriginalText = m_Text.text;
        m_Text.text = "";
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameOver)
        {
            m_Enabled = false;
            m_Text.text = "";
        }

        if (m_Enabled)
        {
            m_Alpha = Mathf.Min(m_Alpha + Time.deltaTime, 1f);
        }
        else
        {
            m_Alpha = Mathf.Max(m_Alpha - Time.deltaTime, 0f);
        }

        if (m_Enabled && Time.time - m_LastLetter > m_LetterInterval)
        {
            if (m_LetterIndex == m_OriginalText.Length - 2)
            {
                m_ResetTime = Time.time;
            }

            if (m_LetterIndex >= m_OriginalText.Length)
            {
                if (Time.time - m_ResetTime > m_ResetDelay)
                {
                    GameManager.Instance.StartMenu();
                }
            }
            else
            {
                m_LastLetter = Time.time;
                m_LetterIndex++;
            }
        }

        if (!m_Enabled && m_LetterIndex > 0 && Time.time - m_LastLetter > m_LetterInterval * 0.3f)
        {
            m_LastLetter = Time.time;
            m_LetterIndex--;
        }

        m_Text.text = m_OriginalText.Substring(0, m_LetterIndex);
        m_Panel.color = new Color(m_PanelColor.r, m_PanelColor.g, m_PanelColor.b, m_Alpha);
    }

    public void Enable(bool enabled)
    {
        m_Alpha = 0f;
        m_Enabled = enabled;
    }
}
