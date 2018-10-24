using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
public class CameraMain : MonoBehaviour
{
    public static Camera Camera { get; private set; }

    private void Awake()
    {
        if (Camera)
            return;

        Camera = GetComponent<Camera>();
    }

    public void ResetCamera()
    {
        Camera.fieldOfView = 45;
        Camera.transform.localPosition = Vector3.zero;
        Camera.transform.eulerAngles = Vector3.zero;
    }
}