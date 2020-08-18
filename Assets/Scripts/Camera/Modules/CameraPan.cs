using UnityEngine;
using UnityEngine.InputSystem;
using Sharp.UI;
using Sharp.Editor;
using Sharp.Core;
using Sharp.Core.Variables;

namespace Sharp.Camera
{
    public class CameraPan : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private KeyVariable[] keys;

        private void Update()
        {
            if (UIUtility.IsInput)
                return;

            var move = Vector2.zero;
            for (var i = 0; i < 4; i++)
                if (Keyboard.current[keys[i]].isPressed)
                    move += Constants.Directions[i];

            move *= speed;
            if (move != Vector2.zero)
                CameraManager.Move(EditorGrid.Clamp(CameraManager.Position + move), .35f);
        }
    }
}
