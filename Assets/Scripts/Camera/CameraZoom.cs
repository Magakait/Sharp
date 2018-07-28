using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float scale;
    public float zoom;

    private float minFOV;
    private float maxFOV;

    private void Awake()
    {
        minFOV = CameraManager.Camera.fieldOfView;
        maxFOV = minFOV + zoom;
    }

    private void OnDestroy() =>
        CameraManager.Camera.fieldOfView = minFOV;

    private void Update()
    {
        if (EngineUtility.IsOverUI)
            return;

        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0)
            CameraManager.Camera.fieldOfView = Mathf.Clamp
                (
                    CameraManager.Camera.fieldOfView - scroll * scale,
                    minFOV,
                    maxFOV
                );
    }
}