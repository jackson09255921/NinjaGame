using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public Canvas pauseCanvas;
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
                pauseCanvas.gameObject.SetActive(true);
                Time.timeScale = 0;
                break;
            }
            case GameState.Chest:
            {
                state = GameState.Play;
                Time.timeScale = 1;
                break;
            }
            case GameState.Pause:
            {
                state = GameState.Play;
                pauseCanvas.gameObject.SetActive(false);
                Time.timeScale = 1;
                break;
            }
        }
    }

    public enum GameState {
            Play,
            Pause,
            Chest,
    }
}
