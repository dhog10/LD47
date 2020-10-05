using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeBlack : MonoBehaviour
{
    public static FadeBlack Instance;

    private Image m_Image;
    private bool m_Faded;
    private float m_Fade;

    public bool m_FullyFaded
        => m_Fade >= 0.999f;

    private void Awake()
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
        m_Image = this.GetComponent<Image>();

        if (Instance != null && Instance != this)
        {
            GameObject.DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        var target = m_Faded ? 1f : 0f;

        m_Fade += (target - m_Fade) * Time.deltaTime * 4f;
        m_Image.color = new Color(m_Image.color.r, m_Image.color.g, m_Image.color.b, m_Fade);
    }

    public void Fade()
    {
        m_Faded = true;
    }

    public void Unfade()
    {
        m_Faded = false;
    }
}
