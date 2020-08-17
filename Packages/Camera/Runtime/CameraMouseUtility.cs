using UnityEngine;
using UnityEngine.InputSystem;

namespace AlKaitagi.Camera
{
    public static class CameraMouseUtility
    {
        public static Vector2 GetMouseOnScreen(float clamp = 0)
        {
            var screen = new Vector2(Screen.width, Screen.height);
            var mouse = Mouse.current.position.ReadValue() - screen / 2;

            if (clamp > 0)
            {
                var radius = clamp * Mathf.Min(screen.x, screen.y);
                if (mouse.magnitude > radius)
                    mouse = radius * mouse.normalized;
            }

            return new Vector3(mouse.x / screen.x, mouse.y / screen.y);
        }
    }
}
