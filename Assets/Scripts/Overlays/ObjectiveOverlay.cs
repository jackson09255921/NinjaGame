using System;
using TMPro;
using UnityEngine;

public class ObjectiveOverlay : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public RectTransform requiredItemPanel;
    public ItemImage requiredItemIconPrefab;
    public CanvasGroup canvasGroup;
    public float visibleDuration;
    public float fadeDuration;
    public float maxDeltaTime = 0.04f;
    float duration;
    float time;

    void Start()
    {
        if (ItemManager.Instance.requiredItems.Length > 0)
        {
            messageText.text = "Collect the following items";
        }
        else
        {
            messageText.text = "Reach the goal";
        }
        int extra = ItemManager.Instance.extraRequirement;
        messageText.text += extra switch
        {
            1 => $", and {extra} additional item",
            >1 => $", and {extra} additional items",
            _ => "",
        };
        messageText.text += ":";
        foreach (ItemManager.Item item in ItemManager.Instance.requiredItems)
        {
            ItemImage image = Instantiate(requiredItemIconPrefab, requiredItemPanel);
            image.Item = item;
        }
        duration = visibleDuration + fadeDuration;
    }

    void Update()
    {
        if (canvasGroup != null)
        {
            time += Math.Min(Time.deltaTime, maxDeltaTime);
            if (time > visibleDuration)
            {
                float prop = (time-visibleDuration)/fadeDuration;
                canvasGroup.alpha = 1-prop*prop;
            }
            if (time > duration)
            {
                canvasGroup.gameObject.SetActive(false);
            }
        }
    }
}