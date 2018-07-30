using UnityEngine;

public class CameraTilt : MonoBehaviour
{
    public float scale;

    private Vector2 mousePosition;
    private Vector2 cameraPosition;

    private void Start()
    {
        mousePosition = .5f * Vector2.one;
        cameraPosition = CameraManager.Position;
    }

    private void Update()
    {
        if (CameraManager.Position != cameraPosition)
            Start();
        else
        {
            Vector2 mousePosition = CameraManager.Camera.ScreenToViewportPoint(Input.mousePosition);
            CameraManager.Position += scale * (mousePosition - this.mousePosition);
            this.mousePosition = mousePosition;
        }
        
        cameraPosition = CameraManager.Position;
    }
}