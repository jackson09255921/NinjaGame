using System.Collections;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public HUD hud;
    public ChestMenu chestMenu;
    public PauseMenu pauseMenu;
    public ResultMenu resultMenu;
    public FadeTransition fadeTransition;
    public float startFadeTime = 0.5f;
    public bool startFadeToLeft;
    InputManager inputManager;
    InputAction escapeAction;
    GameState state = GameState.Start;
    float gameTime;
    float totalTime;
    CinemachineVirtualCamera virtualCamera;
    public string GameTimeText {get; internal set;} = "00:00.00";
    public string TotalTimeText {get; internal set;} = "00:00.00";

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
        fadeTransition.StartFade(startFadeToLeft ? FadeTransition.FadeType.ToLeft : FadeTransition.FadeType.ToRight, startFadeTime, PlayGame);
    }

    void Update()
    {
        if (escapeAction.WasPerformedThisFrame())
        {
            UpdateEscape();
        }
        float deltaTime = Time.unscaledDeltaTime;
        if (Time.timeScale != 0)
        {
            gameTime += deltaTime;
            GameTimeText = $"{(int)gameTime/60:00}:{gameTime%60:00.00}";
        }
        if (state is GameState.Play or GameState.Chest or GameState.Transition)
        {
            totalTime += deltaTime;
            TotalTimeText = $"{(int)totalTime/60:00}:{totalTime%60:00.00}";
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
                player.collectedItems.AddRange(chest.itemIds);
                hud.UpdateItems(chest.itemIds);
            }
            if (chest.weapon != null)
            {
                if (player.inactiveWeapon == null)
                {
                    (player.inactiveWeapon, chest.weapon) = (chest.weapon, null);
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
            resultMenu.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    } 

    internal void EnterGoal(Player player, bool startFadeFromRight)
    {
        if (HasRequiredItems(player))
        {
            state = GameState.Result;
            resultMenu.PlayerComplete("Level Complete");
            resultMenu.gameObject.SetActive(true);
            resultMenu.startFadeFromRight = startFadeFromRight;
        }
        else
        {
            StartCoroutine(StartTransition(player, startFadeFromRight));
        }
        Time.timeScale = 0;
    }

    IEnumerator StartTransition(Player player, bool startFadeFromRight)
    {
        state = GameState.Transition;
        virtualCamera.enabled = false;
        // TODO display text
        yield return new WaitForSecondsRealtime(1);
        fadeTransition.StartFade(startFadeFromRight ? FadeTransition.FadeType.FromRight : FadeTransition.FadeType.FromLeft, startFadeTime, () => StartCoroutine(MidTransition(player)));
    } 

    IEnumerator MidTransition(Player player)
    {
        player.rb.velocity = Vector2.zero;
        player.animator.SetFloat("Speed", 0);
        player.transform.SetPositionAndRotation(player.startPosition, player.startRotation);
        virtualCamera.enabled = true;
        Time.timeScale = 1;
        yield return 0; // Snap camera back to start
        Time.timeScale = 0;
        fadeTransition.StartFade(startFadeToLeft ? FadeTransition.FadeType.ToLeft : FadeTransition.FadeType.ToRight, startFadeTime, PlayGame);
    }

    internal bool HasRequiredItems(Player player)
    {
        return ItemManager.Instance.requiredItems.Select(i => i.id).All(player.collectedItems.Contains);
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
