using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class LightControl : MonoBehaviour
{
    public float startIntensity = 2;
    public float endIntensity = 0;
    public float dimTime = 30;
    public bool debug;
    InputManager inputManager;
    InputAction toggleAction;
    InputAction adjustAction;
    Light2D lightComponent;
    float dimProgress;
    float dimSpeed;
    bool started = false;

    void Awake()
    {
        lightComponent = GetComponent<Light2D>();
        dimSpeed = 1/dimTime;
    }

    void Start()
    {
        inputManager = InputManager.Instance;
        toggleAction = inputManager.FindDebugAction("Light Toggle");
        adjustAction = inputManager.FindDebugAction("Light Adjust");
        dimProgress = 0;
        lightComponent.intensity = startIntensity;
        if (!debug)
        {
            started = true;
        }
    }

    void Update()
    {
        if (debug) {
            if (toggleAction.WasPerformedThisFrame())
            {
                started = !started;
            }
            float adjust = -adjustAction.ReadValue<float>()*0.01f;
            dimProgress += adjust;
        }
        if (started)
        {
            dimProgress -= dimSpeed*Time.deltaTime;
        }
        dimProgress = Math.Clamp(dimProgress, 0, 1);
        lightComponent.intensity = Mathf.Lerp(startIntensity, endIntensity, dimProgress);
    }
}
