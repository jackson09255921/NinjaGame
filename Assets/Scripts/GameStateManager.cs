using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public Canvas pauseCanvas;
    bool paused = false;
    InputManager inputManager;
    InputAction pauseAction;

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
        pauseAction = inputManager.FindAction("Default/Pause");
    }

    void Update()
    {
        if (pauseAction.WasPerformedThisFrame())
        {
            paused = !paused; 
            pauseCanvas.gameObject.SetActive(paused);
            
            if (paused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1; 
            }
        }
    }
}
