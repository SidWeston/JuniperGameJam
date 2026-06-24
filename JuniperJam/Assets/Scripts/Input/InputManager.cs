using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInput;

public class InputManager : MonoBehaviour, IPlayerActions
{
    public static InputManager instance;
    private PlayerInput input;

    //movement events
    public event Action<Vector2> moveEvent;
    public event Action<Vector2> mouseEvent;

    //input keys
    public InputKey shootKey = new InputKey();
    public InputKey windupKey = new InputKey();
    public InputKey interactKey = new InputKey();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        Init();

        DontDestroyOnLoad(this);
    }

    public void Init()
    {
        if(input == null)
        {
            input = new PlayerInput();
            input.Player.SetCallbacks(this);
        }

        input.Player.Enable();
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if(context.ReadValue<float>() > 0 && shootKey.downCounter == 0)
        {
            shootKey.downCounter++;
            shootKey.InvokeKeyPress(true);
        }
        else if(context.ReadValue<float>() <= 0)
        {
            shootKey.downCounter = 0;
            shootKey.InvokeKeyPress(false);
        }
    }

    public void OnWindup(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0 && windupKey.downCounter == 0)
        {
            windupKey.downCounter++;
            windupKey.InvokeKeyPress(true);
        }
        else if (context.ReadValue<float>() <= 0)
        {
            windupKey.downCounter = 0;
            windupKey.InvokeKeyPress(false);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0 && interactKey.downCounter == 0)
        {
            interactKey.downCounter++;
            interactKey.InvokeKeyPress(true);
        }
        else if (context.ReadValue<float>() <= 0)
        {
            interactKey.downCounter = 0;
            interactKey.InvokeKeyPress(false);
        }
    }
}

public class InputKey
{
    public event Action<bool> keyPress;

    public int downCounter = 0;

    public void InvokeKeyPress(bool input)
    {
        keyPress?.Invoke(input);
    }
}
