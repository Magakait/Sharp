using UnityEngine;

public class CameraTilt : MonoBehaviour
{
    [SerializeField]
    private float scale;

    private static readonly Vector3 offset = new Vector3(.5f, .5f, 0);

    private void Update()
    {
        var tilt = scale * (CameraMain.Camera.ScreenToViewportPoint(Input.mousePosition) - offset);
        CameraMain.Camera.transform.localEulerAngles = new Vector3(-tilt.y, tilt.x);
    }
}