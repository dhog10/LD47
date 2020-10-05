using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string m_GameScene;

    private bool m_Paused;
    private Usable[] m_Usables;
    private Holdable[] m_Holdables;
    private float m_LastSearch;
    private bool m_GameOver;

    public bool IsGameOver
        => m_GameOver;

    public bool Paused
        => m_Paused;

    public Usable[] Usables
        => m_Usables;

    public Holdable[] Holdables
        => m_Holdables;

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
        Instance = this;
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

    public Usable GetFocusedUsable(ArrayList unavailable = null)
    {
        var player = TankCharacterController.Instance;
        var minDistance = Mathf.Infinity;
        Usable minUsable = null;

        foreach (var usable in this.Usables)
        {
            if (usable == null || usable.gameObject == null || !usable.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (usable.IsDisabled())
            {
                continue;
            }

            if (unavailable != null && unavailable.Contains(usable))
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

            if (!holdable.CanHold())
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

    public void EndGame(bool win)
    {
        Debug.Log("EndGame " + win);

        m_GameOver = true;

        if (win)
        {

        }
        else
        {
            GameOver.Instance.Enable(true);
        }
    }

    public void StartGame(bool force = false)
    {
        if (!force && !m_GameOver)
        {
            return;
        }

        m_GameOver = false;
        SceneManager.LoadScene(m_GameScene, LoadSceneMode.Single);

        this.SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        SoundManager.Instance?.ResetSounds();
        TankCharacterController.Instance.Revive();

        var spawns = GameObject.FindObjectsOfType<SpawnPoint>();
        if (spawns.Length == 0)
        {
            return;
        }

        var spawn = spawns[Random.Range(0, spawns.Length)];
        TankCharacterController.Instance.transform.position = spawn.transform.position;
    }

    private void ProcessInteractions()
    {
        if (Input.GetButtonDown("Use"))
        {
            var attempts = 0;
            var used = new ArrayList();

            while (attempts < 5)
            {
                var usable = this.GetFocusedUsable(used);

                if (usable != null)
                {
                    if (usable.Toggle())
                    {
                        break;
                    }
                    else
                    {
                        used.Add(usable);
                    }
                }

                attempts++;
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
