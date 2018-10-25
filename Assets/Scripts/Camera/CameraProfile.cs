using UnityEngine;

public class CameraProfile : MonoBehaviour
{
    [SerializeField]
    private bool pan;
    [SerializeField]
    private bool tilt;
    [SerializeField]
    private bool zoom;

    private void Start()
    {
        CameraManager.Camera.GetComponentInChildren<CameraPan>().enabled = pan;
        CameraManager.Camera.GetComponentInChildren<CameraTilt>().enabled = tilt;
        CameraManager.Camera.GetComponentInChildren<CameraZoom>().enabled = zoom;

        Destroy(gameObject);
    }
}