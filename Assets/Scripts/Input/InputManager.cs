using UnityEngine;

[DefaultExecutionOrder(-99)]
public class InputManager : MonoBehaviour
{

    private InputSystem_Actions _actions;
    public InputSystem_Actions Actions => _actions;

    private void Awake()
    {
        if(_actions == null)
            _actions = new InputSystem_Actions();

        _actions.Enable();

        if(GameServices.Input != this)
            GameServices.Input = this;
    }
    private void OnDisable()
    {
        _actions.Disable();

        GameServices.Input = null;
    }
    public void TogglePlayerInput(bool value) 
    {
        if (value) 
        {
            _actions.Player.Disable();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else 
        {
            _actions.Player.Enable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
    }
}
