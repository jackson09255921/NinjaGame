using TMPro;
using UnityEngine;

public class ObjectiveOverlay : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public RectTransform requiredItemPanel;
    public ItemImage requiredItemIconPrefab;

    void Start()
    {
        
        foreach (ItemManager.Item item in ItemManager.Instance.requiredItems)
        {
            ItemImage image = Instantiate(requiredItemIconPrefab, requiredItemPanel);
            image.Item = item;
        }
    }
}