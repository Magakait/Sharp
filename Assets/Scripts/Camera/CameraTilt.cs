using UnityEngine;

public class CameraTilt : MonoBehaviour
{
    [SerializeField]
    private float scale;

    private static readonly Vector3 offset = new Vector3(.5f, .5f, 0);

    private void Update() =>
        CameraManager.Camera.transform.localPosition = scale *
            (CameraManager.Camera.ScreenToViewportPoint(Input.mousePosition) - offset);

    private void OnDisable() =>
        CameraManager.Camera.transform.localPosition = Vector3.zero;
}