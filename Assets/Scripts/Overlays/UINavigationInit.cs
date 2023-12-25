using UnityEngine;
using UnityEngine.EventSystems;

public class UINavigationInit : MonoBehaviour
{
    public GameObject firstNavigationObject;

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstNavigationObject);
    }
}