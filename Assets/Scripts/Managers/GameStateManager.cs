using System.Collections;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public string levelName;
    public string nextSceneName;
    public HUD hud;
    public ChestMenu chestMenu;
    public PauseMenu pauseMenu;
    public Canvas incompleteInfo;
    public ResultMenu resultMenu;
    public FadeTransition fadeTransition;
    public float fadeTime = 0.5f;
    public FadeTransition.Direction fadeDirection = FadeTransition.Direction.Right;
    InputManager inputManager;
    InputAction escapeAction;
    GameState state = GameState.Start;
    int clearItemCount = 0;
    float totalTime;
    float gameTime;
    CinemachineVirtualCamera virtualCamera;

    public string TotalTimeText {get; internal set;} = "00:00.00";
    public string GameTimeText {get; internal set;} = "00:00.00";

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
        virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
        Time.timeScale = 0;
        fadeTransition.StartFade(fadeDirection, false, fadeTime, PlayGame);
        totalTime = ResultManager.Instance.GetTotalTime(levelName);
        TotalTimeText = $"{(int)totalTime/60:00}:{totalTime%60:00.00}";
        gameTime = 0;
    }

    void Update()
    {
        if (escapeAction.WasPerformedThisFrame())
        {
            UpdateEscape();
        }
        float deltaTime = Time.unscaledDeltaTime;
        if (state is GameState.Play or GameState.Chest or GameState.Transition)
        {
            totalTime += deltaTime;
            TotalTimeText = $"{(int)totalTime/60:00}:{totalTime%60:00.00}";
        }
        if (Time.timeScale != 0)
        {
            gameTime += deltaTime;
            GameTimeText = $"{(int)gameTime/60:00}:{gameTime%60:00.00}";
        }
    }

    internal void PlayGame()
    {
        state = GameState.Play;
        Time.timeScale = 1;
    }

    internal void UpdateEscape()
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
                pauseMenu.Reset();
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
                player.Heal(chest.healAmount);
                player.collectedItems.AddRange(chest.items);
                chest.UpdateHint();
                hud.UpdateItems(chest.items);
            }
            if (chest.weapon != null)
            {
                if (player.inactiveWeapon == null)
                {
                    (player.inactiveWeapon, chest.weapon) = (chest.weapon, null);
                    chest.UpdateHint();
                    hud.UpdateEquipment(player.activeWeapon, player.inactiveWeapon);
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

    internal void PlayerDied(Player player)
    {
        if (player.health <= 0)
        {
            state = GameState.Result;
            resultMenu.PlayerDied();
            resultMenu.fadeDirection = FadeTransition.Direction.Alpha;
            resultMenu.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    } 

    internal void EnterGoal(Player player, FadeTransition.Direction fadeDirection)
    {
        if (HasRequiredItems(player))
        {
            state = GameState.Result;
            clearItemCount = player.collectedItems.Count;
            resultMenu.PlayerComplete($"Level complete in {TotalTimeText}, {clearItemCount} / {ItemManager.Instance.totalItemCount} items collected");
            resultMenu.fadeDirection = fadeDirection;
            resultMenu.gameObject.SetActive(true);
        }
        else
        {
            StartCoroutine(StartTransition(player, fadeDirection));
        }
        Time.timeScale = 0;
    }

    IEnumerator StartTransition(Player player, FadeTransition.Direction fadeDirection)
    {
        state = GameState.Transition;
        incompleteInfo.gameObject.SetActive(true);
        virtualCamera.enabled = false;
        // TODO display text
        yield return new WaitForSecondsRealtime(1);
        fadeTransition.StartFade(fadeDirection, true, fadeTime, () => StartCoroutine(MidTransition(player)));
    } 

    IEnumerator MidTransition(Player player)
    {
        incompleteInfo.gameObject.SetActive(false);
        player.rb.velocity = Vector2.zero;
        player.animator.SetFloat("Speed", 0);
        player.transform.SetPositionAndRotation(player.startPosition, player.startRotation);
        Time.timeScale = 1;
        virtualCamera.enabled = true;
        yield return 0; // Snap camera back to start
        Time.timeScale = 0;
        fadeTransition.StartFade(fadeDirection, false, fadeTime, PlayGame);
    }

    internal bool HasRequiredItems(Player player)
    {
        ItemManager im = ItemManager.Instance;
        return im.requiredItems.All(player.collectedItems.Contains) && player.collectedItems.Count >= im.requiredItems.Length + im.extraRequirement;
    }

    internal void RestartResults()
    {
        if (clearItemCount > 0)
        {
            ResultManager.Instance.Reset(levelName);
        }
        else
        {
            ResultManager.Instance.Restart(levelName, totalTime);
        }
    }

    internal void ClearResults()
    {
        ResultManager.Instance.Clear(levelName, totalTime, clearItemCount, ItemManager.Instance.totalItemCount);
    }

    public enum GameState
    {
        Start,
        Play,
        Pause,
        Chest,
        Transition,
        Result,
    }
}
