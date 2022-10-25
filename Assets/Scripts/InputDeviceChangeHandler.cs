using UnityEngine;
using UnityEngine.InputSystem;

public class InputDeviceChangeHandler : MonoBehaviour
{
    private void OnKeyboardSwitch(InputValue ar_InputValue)
    {
        GameManager.SetControlScheme(ControlScheme.Keyboard);
        Cursor.visible = true;
    }

    private void OnGamepadSwitch(InputValue ar_InputValue)
    {
        GameManager.SetControlScheme(ControlScheme.Gamepad);
        Cursor.visible = false;
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
    }
}
