using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public InputActionAsset actionAsset;

    void Awake()
    {
        Instance = this;
        actionAsset.Enable();
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
        return actionAsset.FindActionMap("Default").FindAction(nameOrId);
    }

    public InputAction FindDebugAction(string nameOrId)
    {
        return actionAsset.FindActionMap("Debug").FindAction(nameOrId);
    }
}
