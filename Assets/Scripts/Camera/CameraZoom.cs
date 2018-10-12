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

    private void Start() => tween = CameraMain.Camera.DOFieldOfView(minFOV, .35f * Constants.Time);

    private void OnDestroy() => tween.Kill();

    private void OnDisable() => tween.Pause();

    private void Update()
    {
        if (EngineUtility.IsOverUI)
            return;

        var scroll = scale * Input.mouseScrollDelta.y;
        if (scroll != 0)
            tween
                .ChangeValues
                (
                    CameraMain.Camera.fieldOfView,
                    Mathf.Clamp(CameraMain.Camera.fieldOfView - scroll, minFOV, maxFOV)
                )
                .Restart();
    }
}