using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField]
    private float scale;

    [Space(10)]
    [SerializeField]
    private float minFOV;
    [SerializeField]
    private float maxFOV;

    private void OnEnable() =>
        CameraManager.Camera.fieldOfView = minFOV;

    private void OnDisable() =>
        OnEnable();

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