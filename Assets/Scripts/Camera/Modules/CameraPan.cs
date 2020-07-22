using UnityEngine;
using UnityEngine.InputSystem;
using Sharp.UI;
using Sharp.Core.Variables;

public class CameraPan : MonoBehaviour
{
    [SerializeField]
    private float scale;
    [SerializeField]
    private KeyVariable[] keys;

    private void Update()
    {
        if (UIUtility.IsInput)
            return;

        var move = Vector2.zero;
        for (var i = 0; i < 4; i++)
            if (Keyboard.current[keys[i]].isPressed)
                move += Constants.Directions[i];

        move *= scale;
        if (move != Vector2.zero)
            CameraManager.Move(EditorGrid.Clamp(CameraManager.Position + move), .35f);
    }
}
