using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance { get; private set; }
    public float dimTime = 30;
    public LightDimEntry[] lightDimEntries;
    public bool debug;
    InputManager inputManager;
    InputAction toggleAction;
    InputAction adjustAction;
    float dimProgress;
    float dimSpeed;
    bool started = false;

    void Awake()
    {
        Instance = this;
        dimSpeed = 1/dimTime;
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
        if (debug)
        {
            inputManager.EnableActionMap("Debug");
        }
        toggleAction = inputManager.FindAction("Debug/Light Toggle");
        adjustAction = inputManager.FindAction("Debug/Light Adjust");
        dimProgress = 0;
        foreach(LightDimEntry entry in lightDimEntries)
        {
            entry.light.intensity = entry.startIntensity;
        }
        started = true;
    }

    void Update()
    {
        if (debug)
        {
            if (toggleAction.WasPerformedThisFrame())
            {
                started = !started;
            }
            float adjust = -adjustAction.ReadValue<float>()*0.01f;
            dimProgress += adjust;
        }
        if (started)
        {
            dimProgress += dimSpeed*Time.deltaTime;
        }
        dimProgress = Math.Clamp(dimProgress, 0, 1);
        foreach(LightDimEntry entry in lightDimEntries)
        {
            entry.light.intensity = Mathf.Lerp(entry.startIntensity, entry.endIntensity, dimProgress);
        }
    }

    [Serializable]
    public struct LightDimEntry
    {
        public Light2D light;
        public float startIntensity;
        public float endIntensity;
    }
}
