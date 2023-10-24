using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public Canvas pauseCanvas;
    private bool isCanvasVisible = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
