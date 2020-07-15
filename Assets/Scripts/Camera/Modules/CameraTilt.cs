using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTilt : MonoBehaviour
{
    [SerializeField]
    private float scale;

    private void Update()
    {
        var mouse = CameraManager.Camera.ScreenToViewportPoint(Mouse.current.position.ReadValue());
        CameraManager.Rotation = scale * new Vector3(.5f - mouse.y, mouse.x - .5f);
    }

    private void OnDisable() => CameraManager.Rotation = Vector3.zero;
}
