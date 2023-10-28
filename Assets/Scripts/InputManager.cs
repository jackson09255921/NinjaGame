using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public InputActionAsset actionAsset;

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
        actionAsset.Disable();
    }

    public InputAction FindAction(string nameOrId)
    {
        return actionAsset.FindAction(nameOrId);
    }

    public void EnableActionMap(string nameOrId)
    {
        actionAsset.FindActionMap(nameOrId).Enable();
    }

    public void DisableActionMap(string nameOrId)
    {
        actionAsset.FindActionMap(nameOrId).Disable();
    }
}
