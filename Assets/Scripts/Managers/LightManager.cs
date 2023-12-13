using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance { get; private set; }
    public LightDimEntry[] lightDimEntries;
    public SpriteFadeEntry[] spriteFadeEntries;
    public GraphicFadeEntry[] graphicFadeEntries;
    public bool debug;
    InputManager inputManager;
    InputAction toggleAction;
    InputAction adjustAction;
    float totalTime;
    float maxTime;
    bool started = false;

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
        if (debug)
        {
            inputManager.EnableActionMap("Debug");
        }
        toggleAction = inputManager.FindAction("Debug/Light Toggle");
        adjustAction = inputManager.FindAction("Debug/Light Adjust");
        totalTime = 0;
        foreach (LightDimEntry entry in lightDimEntries)
        {
            entry.light.intensity = entry.startIntensity;
        }
        foreach (SpriteFadeEntry entry in spriteFadeEntries)
        {
            entry.spriteRenderer.color = entry.startColor;
        }
        foreach (GraphicFadeEntry entry in graphicFadeEntries)
        {
            entry.graphic.color = entry.startColor;
        }
        maxTime = LinqUtility.Concat<float>(lightDimEntries.Select(e => e.dimTime), spriteFadeEntries.Select(e => e.fadeTime), graphicFadeEntries.Select(e => e.fadeTime)).Max();
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
            totalTime += adjust;
        }
        if (started)
        {
            totalTime += Time.deltaTime;
        }
        totalTime = Math.Clamp(totalTime, 0, maxTime);
        foreach (LightDimEntry entry in lightDimEntries)
        {
            entry.light.intensity = Mathf.Lerp(entry.startIntensity, entry.endIntensity, totalTime/entry.dimTime);
        }
        foreach (SpriteFadeEntry entry in spriteFadeEntries)
        {
            entry.spriteRenderer.color = Color.Lerp(entry.startColor, entry.endColor, totalTime/entry.fadeTime);
        }
        foreach (GraphicFadeEntry entry in graphicFadeEntries)
        {
            entry.graphic.color = Color.Lerp(entry.startColor, entry.endColor, totalTime/entry.fadeTime);
        }
    }

    [Serializable]
    public struct LightDimEntry
    {
        public Light2D light;
        public float startIntensity;
        public float endIntensity;
        public float dimTime;
    }

    [Serializable]
    public struct SpriteFadeEntry
    {
        public SpriteRenderer spriteRenderer;
        public Color startColor;
        public Color endColor;
        public float fadeTime;
    }

    [Serializable]
    public struct GraphicFadeEntry
    {
        public Graphic graphic;
        public Color startColor;
        public Color endColor;
        public float fadeTime;
    }
}
