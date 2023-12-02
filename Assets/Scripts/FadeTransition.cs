using System;
using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
    public RectTransform fadeRect;
    public Image fadeLeft;
    public Image fadeCenter;
    public Image fadeRight;
    public Color color;
    public float maxDeltaTime = 0.04f;
    Vector2 leftEnd;
    Vector2 rightEnd;
    FadeType fadeType;
    float fadeProgress;
    float fadeDuration;
    Action endCallback;
    Action<float> cancelCallback;

    public Color Color
    {
        get => color;
        internal set
        {
            fadeLeft.color = fadeCenter.color = fadeRight.color = color = value;
        }
    }

    void Start()
    {
        Color = color;
        OnRectTransformDimensionsChange();
    }

    void Update()
    {
        if (fadeDuration > 0)
        {
            fadeProgress += Math.Min(Time.unscaledDeltaTime, maxDeltaTime);
            float progress = fadeProgress / fadeDuration;
            Vector2 aPos = fadeType switch
            {
                FadeType.ToLeft => Vector2.Lerp(Vector2.zero, leftEnd, progress),
                FadeType.ToRight => Vector2.Lerp(Vector2.zero, rightEnd, progress),
                FadeType.FromLeft => Vector2.Lerp(leftEnd, Vector2.zero, progress),
                FadeType.FromRight => Vector2.Lerp(rightEnd, Vector2.zero, progress),
                _ => Vector2.zero,
            };
            fadeRect.anchoredPosition = aPos;
            if (progress >= 1)
            {
                if (fadeType is FadeType.ToLeft or FadeType.ToRight)
                {
                    gameObject.SetActive(false);
                }
                fadeDuration = fadeProgress = 0;
                endCallback?.Invoke();
            }
        }
    }

    void OnRectTransformDimensionsChange()
    {
        float leftWidth = fadeCenter.rectTransform.rect.width + fadeRight.rectTransform.rect.width;
        float rightWidth = fadeCenter.rectTransform.rect.width + fadeLeft.rectTransform.rect.width;
        leftEnd = new(-leftWidth, 0);
        rightEnd = new(rightWidth, 0);
    }

    internal void StartFade(FadeType type, float duration, Action endCallback, Action<float> cancelCallback = null)
    {
        if (fadeDuration > 0)
        {
            this.cancelCallback?.Invoke(fadeProgress);
        }
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
        fadeType = type;
        fadeProgress = 0;
        fadeDuration = duration;
        this.endCallback = endCallback;
        this.cancelCallback = cancelCallback;
    }

    public enum FadeType
    {
        ToLeft,
        ToRight,
        FromLeft,
        FromRight,
    }
}