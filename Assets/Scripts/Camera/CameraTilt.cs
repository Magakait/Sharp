using UnityEngine;

public class CameraTilt : MonoBehaviour
{
    [SerializeField]
    private float scale;

    private void Awake() => CameraModules.Tilt.scale = scale;

    private void Update()
    {
        var mouse = CameraManager.Camera.ScreenToViewportPoint(Input.mousePosition);
        CameraManager.Rotation = scale * new Vector3(.5f - mouse.y, mouse.x - .5f);
    }
}