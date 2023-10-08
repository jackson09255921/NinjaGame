using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class LightControl : MonoBehaviour
{
    public float maxIntensity = 1.4f;
    public float minIntensity = 0;
    public float gamma = 2;
    public float dimTime = 30;
    public bool debug;
    public InputActionAsset actionAsset;
    InputActionMap debugActionMap;
    InputAction toggleAction;
    InputAction adjustAction;
    Light2D lightComponent;
    float intensity;
    float dimSpeed;
    bool started = false;

    void Awake()
    {
        lightComponent = GetComponent<Light2D>();
        dimSpeed = (maxIntensity-minIntensity)/dimTime;
        if (debug)
        {
            debugActionMap = actionAsset.FindActionMap("Debug");
            toggleAction = debugActionMap.FindAction("Light Toggle");
            adjustAction = debugActionMap.FindAction("Light Adjust");
        }
    }

    void OnEnable()
    {
        debugActionMap?.Enable();
    }

    void OnDisable()
    {
        debugActionMap?.Disable();
    }

    void Start()
    {
        intensity = maxIntensity;
        lightComponent.intensity = MathF.Pow(intensity, gamma);
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
            float adjust = adjustAction.ReadValue<float>()*0.01f;
            intensity += adjust;
        }
        if (started)
        {
            intensity -= dimSpeed*Time.deltaTime;
        }
        intensity = Math.Clamp(intensity, minIntensity, maxIntensity);
        lightComponent.intensity = Math.Max(MathF.Pow(intensity, gamma), 0);
    }
}
