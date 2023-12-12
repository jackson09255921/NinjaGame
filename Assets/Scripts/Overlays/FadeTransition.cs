using System;
using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
    public RectTransform fadeRect;
    public Image fadeCenter;
    public Image fadeLeft;
    public Image fadeRight;
    public Image fadeUp;
    public Image fadeDown;
    public Color color;
    public float maxDeltaTime = 0.04f;
    Vector2 leftEnd;
    Vector2 rightEnd;
    Vector2 upEnd;
    Vector2 downEnd;
    Direction direction;
    bool inbound;
    Vector2 end;
    float progress;
    float duration;
    Action endCallback;
    Action<float> cancelCallback;

    void Start()
    {
        OnRectTransformDimensionsChange();
    }

    void Update()
    {
        if (duration > 0)
        {
            progress += Math.Min(Time.unscaledDeltaTime, maxDeltaTime);
            float prop = progress / duration;
            if (direction == Direction.Alpha)
            {
                SetColor(color * new Color(1, 1, 1, inbound ? prop*(2-prop) : 1-prop*prop));
            }
            else
            {
                fadeRect.anchoredPosition = Vector2.Lerp(Vector2.zero, end, inbound ? 1-prop : prop);
            }
            if (prop >= 1)
            {
                if (!inbound)
                {
                    gameObject.SetActive(false);
                }
                duration = progress = 0;
                endCallback?.Invoke();
            }
        }
    }

    void SetColor(Color color)
    {
        fadeCenter.color = fadeLeft.color = fadeRight.color = fadeUp.color = fadeDown.color = color;
    }

    void OnRectTransformDimensionsChange()
    {
        float leftWidth = fadeCenter.rectTransform.rect.width + fadeRight.rectTransform.rect.width;
        float rightWidth = fadeCenter.rectTransform.rect.width + fadeLeft.rectTransform.rect.width;
        float upHeight = fadeCenter.rectTransform.rect.height + fadeUp.rectTransform.rect.height;
        float downHeight = fadeCenter.rectTransform.rect.height + fadeDown.rectTransform.rect.height;
        leftEnd = new(-leftWidth, 0);
        rightEnd = new(rightWidth, 0);
        upEnd = new(0, -upHeight);
        downEnd = new(0, downHeight);
    }

    internal void StartFade(Direction direction, bool inbound, float duration, Action endCallback, Action<float> cancelCallback = null)
    {
        if (this.duration > 0)
        {
            this.cancelCallback?.Invoke(progress);
        }
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
        SetColor(color);
        fadeRect.anchoredPosition = Vector2.zero;
        this.direction = direction;
        this.inbound = inbound;
        end = direction switch
        {
            Direction.Left => leftEnd,
            Direction.Right => rightEnd,
            Direction.Up => upEnd,
            Direction.Down => downEnd,
            _ => Vector2.zero,
        };
        progress = 0;
        this.duration = duration;
        this.endCallback = endCallback;
        this.cancelCallback = cancelCallback;
    }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
        Alpha
    }

    public Direction Opposite(Direction direction)
    {
        return direction switch
        {
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            _ => Direction.Alpha,
        };
    }
}