using UnityEngine;
using UnityEngine.InputSystem;

namespace Sharp.Camera
{
    public class CameraTilt : MonoBehaviour
    {
        [SerializeField] private float speed;

        private void Update()
        {
            var mouse = CameraManager.Camera.ScreenToViewportPoint(Mouse.current.position.ReadValue());
            CameraManager.Rotation = speed * new Vector3(.5f - mouse.y, mouse.x - .5f);
        }

        private void OnDisable() => CameraManager.Rotation = Vector3.zero;
    }
}
