using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public event Action OnLeftClick;
    public event Action OnRotate;
    public event Action<int> OnNumber;
    public event Action OnInteract;
    public event Action<bool> OnBuildMode;
    public event Action<bool> OnMouseHoverAction;
    InputPlayerAction inputManager;

    void Awake()
    {
        Instance = this;
        inputManager = new InputPlayerAction();
    }

    private void BuildModeOn(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        inputManager.Player.Disable();
        inputManager.BuildMode.Enable();
        Debug.Log("Build Mode On");
        OnBuildMode?.Invoke(true);
    }

    private void BuildModeOff(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        inputManager.BuildMode.Disable();
        inputManager.Player.Enable();
        Debug.Log("Build Mode Off");
        OnBuildMode?.Invoke(false);
    }

    private void MouseLeftButton(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnLeftClick?.Invoke();
    }


    private void Interacts(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnInteract?.Invoke();
    }

    private void PickNumbers(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        var key = context.control.name;
        int numberKey = 0;
        switch (key)
        {
            case "1":
                numberKey = 1;
                break;
            case "2":
                numberKey = 2;
                break;
            case "3":
                numberKey = 3;
                break;
            case "4":
                numberKey = 4;
                break;
            case "5":
                numberKey = 5;
                break;
            case "6":
                numberKey = 6;
                break;
            case "7":
                numberKey = 7;
                break;
            case "8":
                numberKey = 8;
                break;
            case "9":
                numberKey = 9;
                break;

        }
        OnNumber?.Invoke(numberKey);
    }

    private void RotateObject(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnRotate?.Invoke();
    }
    void OnEnable()
    {
        inputManager.Enable();
        inputManager.BuildMode.Disable();

        inputManager.Player.Interact.performed += Interacts;
        inputManager.Player.BuildModeOn.performed += BuildModeOn;

        inputManager.BuildMode.MouseLeftButton.started += MouseActionStarted;
        inputManager.BuildMode.MouseLeftButton.performed += MouseLeftButton;
        inputManager.BuildMode.MouseLeftButton.canceled += MouseActionCanceled;
        inputManager.BuildMode.Rotate.performed += RotateObject;
        inputManager.BuildMode.PickNumbers.performed += PickNumbers;
        inputManager.BuildMode.BuildModeOff.performed += BuildModeOff;
    }
    private void MouseActionStarted(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnMouseHoverAction?.Invoke(true);
    }
    private void MouseActionCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnMouseHoverAction?.Invoke(false);
    }



    void OnDisable()
    {
        inputManager.Disable();

        inputManager.Player.Interact.performed -= Interacts;
        inputManager.Player.BuildModeOn.performed -= BuildModeOn;

        inputManager.BuildMode.MouseLeftButton.performed -= MouseLeftButton;
        inputManager.BuildMode.Rotate.performed -= RotateObject;
        inputManager.BuildMode.PickNumbers.performed -= PickNumbers;
        inputManager.BuildMode.BuildModeOff.performed -= BuildModeOff;
    }
}
