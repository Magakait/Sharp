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
    
    private void Update()
    {
        if (EngineUtility.IsOverUI)
            return;

        var scroll = scale * Input.mouseScrollDelta.y;
        if (scroll != 0)
            CameraManager.Zoom(Mathf.Clamp(CameraManager.FieldOfView - scroll, minFOV, maxFOV));
    }
}