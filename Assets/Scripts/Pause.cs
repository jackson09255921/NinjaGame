using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    public Canvas pauseCanvas;
    private bool isCanvasVisible = false;
    private InputManager inputManager;
    private InputAction pauseAction;

    void Start()
    {
        inputManager = InputManager.Instance;
        pauseAction = inputManager.FindAction("Pause");
    }

    void Update()
    {
        if (pauseAction.WasPerformedThisFrame())
        {
            isCanvasVisible = !isCanvasVisible; 
            pauseCanvas.gameObject.SetActive(isCanvasVisible);
            
            if (isCanvasVisible)
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
