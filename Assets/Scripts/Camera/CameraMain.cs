using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMain : MonoBehaviour
{
    [SerializeField]
    private new Camera camera;
    public static Camera Camera { get; private set; }

    private void Awake()
    {
        if (Camera)
            return;

        Camera = camera;
    }

    public void ResetCamera()
    {
        Camera.fieldOfView = 45;
        Camera.transform.localPosition = Vector3.zero;
        Camera.transform.eulerAngles = Vector3.zero;
    }
}