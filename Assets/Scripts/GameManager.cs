using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private bool m_Paused;
    private Usable[] m_Usables;
    private Holdable[] m_Holdables;
    private float m_LastSearch;

    public bool Paused
        => m_Paused;

    public Usable[] Usables
        => m_Usables;

    public Holdable[] Holdables
        => m_Holdables;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        Instance = this; ;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        this.FindUsables();
        this.FindHoldables();
    }

    private void Update()
    {
        this.ProcessInteractions();

        if (Time.time - m_LastSearch > 1f)
        {
            this.FindHoldables();
            this.FindUsables();

            m_LastSearch = Time.time;
        }
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
        var player = TankCharacterController.Instance;
        var minDistance = Mathf.Infinity;
        Usable minUsable = null;

        foreach (var usable in this.Usables)
        {
            if (usable.IsDisabled())
            {
                continue;
            }

            var distance = Vector3.Distance(usable.transform.position, player.transform.position);

            if (distance < usable.UseDistance && distance < minDistance)
            {
                minDistance = distance;
                minUsable = usable;
            }
        }

        return minUsable;
    }

    public Holdable GetFocusedHoldable()
    {
        var player = TankCharacterController.Instance;
        var minDistance = Mathf.Infinity;
        Holdable minHoldable = null;

        var holding = player.Holdable;

        foreach (var holdable in this.Holdables)
        {
            if (holding == holdable)
            {
                continue;
            }

            var distance = Vector3.Distance(holdable.transform.position, player.transform.position);

            if (distance < holdable.UseDistance && distance < minDistance)
            {
                minDistance = distance;
                minHoldable = holdable;
            }
        }

        return minHoldable;
    }

    private void ProcessInteractions()
    {
        if (Input.GetButtonDown("Use"))
        {
            var usable = this.GetFocusedUsable();

            if (usable != null)
            {
                usable.Toggle();
            }
        }

        var player = TankCharacterController.Instance;
        if (player != null)
        {
            if (Input.GetButtonDown("Hold"))
            {
                var holdable = this.GetFocusedHoldable();

                if (holdable != null)
                {
                    player.Pickup(holdable);
                }
                else if(player.IsHolding)
                {
                    player.Drop();
                }
            }
        }
    }

    private void FindUsables()
    {
        m_Usables = GameObject.FindObjectsOfType<Usable>();
    }

    private void FindHoldables()
    {
        m_Holdables = GameObject.FindObjectsOfType<Holdable>();
    }
}
