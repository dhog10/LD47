using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public UnityEngine.UI.Image m_Frame;
    public UnityEngine.UI.Button m_MenuButton;
    public UnityEngine.UI.Button m_RestartButton;
    public UnityEngine.UI.Button m_QuitButton;

    void Start()
    {
        m_MenuButton.onClick.AddListener(() =>
        {
            if (SceneManager.GetActiveScene().name == GameManager.Instance.m_MenuScene)
            {
                return;
            }

            m_Frame.gameObject.SetActive(false);
            GameManager.Instance.LockCursor();

            GameManager.Instance.StartMenu();
        });

        m_RestartButton.onClick.AddListener(() =>
        {
            if (SceneManager.GetActiveScene().name == GameManager.Instance.m_MenuScene)
            {
                return;
            }

            m_Frame.gameObject.SetActive(false);
            GameManager.Instance.LockCursor();

            GameManager.Instance.EndGame(false);
        });

        m_QuitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        m_Frame.gameObject.SetActive(false);
    }

    void Update()
    {
        if (m_Frame.gameObject.activeSelf && GameManager.Instance.IsGameOver)
        {
            m_Frame.gameObject.SetActive(false);
            GameManager.Instance.LockCursor();
        }

        if (GameManager.Instance.IsGameOver)
        {
            return;
        }

        if (Input.GetButtonDown("Pause"))
        {
            m_Frame.gameObject.SetActive(!m_Frame.gameObject.activeSelf);

            if (m_Frame.gameObject.activeSelf)
            {
                GameManager.Instance.UnlockCursor();
            }
            else
            {
                GameManager.Instance.LockCursor();
            }
        }
    }
}
