using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool m_Paused;
    private Usable[] m_Usables;

    public bool Paused
        => m_Paused;

    public Usable[] Usables
        => m_Usables;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        this.FindUsabled();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        m_Paused = true;
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
        m_Paused = false;
    }

    public Usable GetFocusedUsable()
    {
        foreach (var usable in this.Usables)
        {

        }
    }

    private void FindUsabled()
    {
        m_Usables = GameObject.FindObjectsOfType<Usable>();
    }
}
