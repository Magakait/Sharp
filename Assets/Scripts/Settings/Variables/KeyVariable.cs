using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Variables/Key")]
public class KeyVariable : BaseVariable<Key>
{
    public bool IsDown =>
        Keyboard.current[this].wasPressedThisFrame;

    public bool IsHeld =>
        Keyboard.current[this].isPressed;

    public bool IsUp =>
        Keyboard.current[this].wasPressedThisFrame;
}
