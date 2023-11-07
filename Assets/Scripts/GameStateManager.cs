using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public ChestMenu chestMenu;
    public PauseMenu pauseMenu;
    InputManager inputManager;
    InputAction escapeAction;
    GameState state = GameState.Play;

    void Awake()
    {
        Instance = this;
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void Start()
    {
        inputManager = InputManager.Instance;
        inputManager.EnableActionMap("Default");
        escapeAction = inputManager.FindAction("Default/Escape");
        Time.timeScale = 1;
    }

    void Update()
    {
        if (escapeAction.WasPerformedThisFrame())
        {
            UpdateEscape();
        }
    }

    void UpdateEscape()
    {
        switch (state)
        {
            case GameState.Play:
            {
                state = GameState.Pause;
                pauseMenu.gameObject.SetActive(true);
                Time.timeScale = 0;
                break;
            }
            case GameState.Chest:
            {
                state = GameState.Play;
                chestMenu.gameObject.SetActive(false);
                Time.timeScale = 1;
                break;
            }
            case GameState.Pause:
            {
                state = GameState.Play;
                pauseMenu.gameObject.SetActive(false);
                Time.timeScale = 1;
                break;
            }
        }
    }

    internal void OpenChest(Player player, Chest chest)
    {
        if (state == GameState.Play)
        {
            if (!chest.Open)
            {
                chest.Open = true;
            }
            if (chest.weapon != null)
            {
                if (player.inactiveWeapon == null)
                {
                    player.inactiveWeapon = chest.weapon;
                    chest.weapon = null;
                }
                else
                {
                    state = GameState.Chest;
                    chestMenu.SetContents(player, chest);
                    chestMenu.gameObject.SetActive(true);
                    Time.timeScale = 0;
                }
            }
        }
    }

    public enum GameState {
            Play,
            Pause,
            Chest,
            Result,
    }
}
