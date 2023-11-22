using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public ChestMenu chestMenu;
    public PauseMenu pauseMenu;
    public ResultMenu resultMenu;
    InputManager inputManager;
    InputAction escapeAction;
    GameState state = GameState.Play;
    float gameTime;
    float totalTime;
    public string GameTimeText {get; internal set;}
    public string TotalTimeText {get; internal set;}

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
        if (Time.timeScale != 0)
        {
            gameTime += Time.unscaledDeltaTime;
            GameTimeText = $"{(int)gameTime/60:00}:{gameTime%60:00.00}";
        }
        if (state != GameState.Pause && state != GameState.Result)
        {
            totalTime += Time.unscaledDeltaTime;
            TotalTimeText = $"{(int)totalTime/60:00}:{totalTime%60:00.00}";
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
                player.collectedItems.AddRange(chest.itemIds);
                player.hud.UpdateItems(chest.itemIds);
            }
            if (chest.weapon != null)
            {
                if (player.inactiveWeapon == null)
                {
                    (player.inactiveWeapon, chest.weapon) = (chest.weapon, null);
                    player.hud.UpdateEquipment(player.activeWeapon, player.inactiveWeapon);
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

    internal void ShowResults(Player player)
    {
        state = GameState.Result;
        if (player.health <= 0)
        {
            resultMenu.PlayerDied();
        }
        else if (HasRequiredItems(player))
        {
            resultMenu.PlayerComplete("Level Complete");
        }
        else
        {
            resultMenu.PlayerIncomplete("You did not collect all required items.");
        }
        resultMenu.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    internal bool HasRequiredItems(Player player)
    {
        return ItemManager.Instance.requiredItems.Select(i => i.id).All(player.collectedItems.Contains);
    }

    public enum GameState {
            Play,
            Pause,
            Chest,
            Result,
    }
}
