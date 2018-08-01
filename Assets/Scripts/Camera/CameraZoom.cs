using UnityEngine;

using DG.Tweening;

public class CameraZoom : MonoBehaviour
{
    [SerializeField]
    private float scale;

    [Space(10)]
    [SerializeField]
    private float minFOV;
    [SerializeField]
    private float maxFOV;

    private Tweener tween;

    private void Awake()
    {
        CameraManager.Camera.fieldOfView = minFOV;
        
        tween = CameraManager.Camera
            .DOFieldOfView(minFOV, .25f * Constants.Time)
            .Pause();
    }

    private void OnDestroy() =>
        tween.Kill();

    private void OnEnable() =>
        CameraManager.Camera.fieldOfView = minFOV;

    private void OnDisable()
    {
        OnEnable();
        tween.Pause();
    }

    private void Update()
    {
        if (EngineUtility.IsOverUI)
            return;

        var scroll = scale * Input.mouseScrollDelta.y;
        if (scroll != 0)
            tween
                .ChangeValues
                (
                    CameraManager.Camera.fieldOfView,
                    Mathf.Clamp(CameraManager.Camera.fieldOfView - scroll, minFOV, maxFOV)
                )
                .Restart();
    }
}