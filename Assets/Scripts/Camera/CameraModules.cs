using UnityEngine;

[RequireComponent(typeof(CameraFollow))]
[RequireComponent(typeof(CameraPan))]
[RequireComponent(typeof(CameraTilt))]
[RequireComponent(typeof(CameraZoom))]
public class CameraModules : MonoBehaviour
{
    public static CameraFollow Follow { get; private set; }
    public static CameraPan Pan { get; private set; }
    public static CameraTilt Tilt { get; private set; }
    public static CameraZoom Zoom { get; private set; }

    private static bool first;

    private void Awake()
    {
        if (first)
            return;

        Follow = GetComponent<CameraFollow>();
        Pan = GetComponent<CameraPan>();
        Tilt = GetComponent<CameraTilt>();
        Zoom = GetComponent<CameraZoom>();
    }

    private void Start()
    {
        if (first)
            Destroy(gameObject);
        first = true;
    }
}