using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UICancelListener : MonoBehaviour
{
    static DefaultInputActions inputActions;

    public UnityEvent onCancel;

    void Start()
    {
        if (inputActions == null)
        {
            inputActions = new();
            inputActions.UI.Cancel.Enable();
        }
    }

    void Update()
    {
        if (inputActions.UI.Cancel.WasPerformedThisFrame())
        {
            onCancel.Invoke();
        }
    }
}
