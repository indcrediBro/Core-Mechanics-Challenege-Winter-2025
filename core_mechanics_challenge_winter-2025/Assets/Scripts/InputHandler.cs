using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public                   UnityEvent           fireEvent = new UnityEvent();
    
    [SerializeField] private InputActionReference fireInputAction;
    [SerializeField] private InputActionReference mousePositionEventAction;
    [SerializeField] private InputActionReference joystickPositionEventAction;
    [SerializeField] private InputActionReference movementEventAction;

    private bool usingMouse = true;
    

    void OnEnable()
    {
        setUpInputActions();
    }

    
    private void OnDisable()
    {
        breakDownInputActions();
    }


    void setUpInputActions()
    {
        fireInputAction.action.performed          += Fire;
        mousePositionEventAction.action.performed += mouseMoved;
        joystickPositionEventAction.action.performed += joystickMoved;
        
        
        fireInputAction.action.Enable();        
        mousePositionEventAction.action.Enable();
        joystickPositionEventAction.action.Enable();
        movementEventAction.action.Enable();
    }

    private void joystickMoved(InputAction.CallbackContext obj)
    {
        usingMouse = false;
    }

    private void mouseMoved(InputAction.CallbackContext obj)
    {
        usingMouse = true;
    }


    private void breakDownInputActions()
    {
        fireInputAction.action.Disable();
        mousePositionEventAction.action.Disable();
        joystickPositionEventAction.action.Disable();
        movementEventAction.action.Disable();
    }
    
    

    private void Fire(InputAction.CallbackContext obj)
    {
        fireEvent.Invoke();
    }


    public Vector2 GetMouse()
    {
        Vector2 pos = mousePositionEventAction.action.ReadValue<Vector2>();
        return pos;
    }


    public Vector2 GetDirection()
    {
        Vector2 dir;
        if (usingMouse)
        {
            dir =  Camera.main.ScreenToWorldPoint(GetMouse()-new Vector2(.5f, .5f));
            Debug.Log(dir);
        }
        else
        {
            dir = joystickPositionEventAction.action.ReadValue<Vector2>();
            Debug.Log(dir);
        }
        return dir;
    }
    
    public Vector2 GetMovement()
    {
        return movementEventAction.action.ReadValue<Vector2>();
    }
}
