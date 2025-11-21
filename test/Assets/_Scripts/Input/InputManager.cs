using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public event Action OnLeftClick;
    public event Action OnRotate;
    public event Action<int> OnNumber;
    InputPlayerAction inputManager;

    void Awake()
    {
        Instance = this;
        inputManager = new InputPlayerAction();
    }
    
    private void MouseLeftClick(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnLeftClick?.Invoke();
    }
    void OnEnable()
    {
        inputManager.Enable();
        inputManager.Player.MouseLeftButton.performed += MouseLeftClick;
        inputManager.Player.Rotate.performed +=RotateObject;
        inputManager.Player.PickNumbers.performed += PickNumbers;

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

    void OnDisable()
    {
        inputManager.Disable();
        inputManager.Player.MouseLeftButton.performed -= MouseLeftClick;
        inputManager.Player.Rotate.performed -=RotateObject;
        inputManager.Player.PickNumbers.performed -= PickNumbers;
    }
}
