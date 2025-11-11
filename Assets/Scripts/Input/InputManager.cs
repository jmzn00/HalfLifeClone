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
}
