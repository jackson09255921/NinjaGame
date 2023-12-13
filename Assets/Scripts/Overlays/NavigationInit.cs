using UnityEngine;
using UnityEngine.EventSystems;

public class NavigationInit : MonoBehaviour
{
    public GameObject firstNavigationObject;

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstNavigationObject);
    }
}