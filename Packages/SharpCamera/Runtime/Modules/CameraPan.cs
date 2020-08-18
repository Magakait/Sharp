using UnityEngine;
using UnityEngine.InputSystem;
using Sharp.UI;
using Sharp.Core;
using Sharp.Core.Variables;

namespace Sharp.Camera
{
    public class CameraPan : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private KeyVariable[] keys;

        public static float Side { get; set; }

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
                CameraManager.Move(Clamp(CameraManager.Position + move), .25f);
        }

        public static Vector2 Clamp(Vector2 point)
        {
            var halfSide = Side / 2;
            point.x = Mathf.Clamp(point.x, -halfSide, halfSide);
            point.y = Mathf.Clamp(point.y, -halfSide, halfSide);
            return point;
        }
    }
}
