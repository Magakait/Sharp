using UnityEngine;

using DG.Tweening;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private float scale;
    [SerializeField]
    private KeyVariable[] keys;

    private Tweener tween;

    private void Awake() =>
        tween = DOTween
            .To
            (
                () => CameraManager.Position, 
                v => CameraManager.Position = v, 
                Vector2.zero, 
                .25f * Constants.Time
            )
            .Pause();

    private void OnDestroy() =>
        tween.Kill();

    private void Update()
    {
        if (EngineUtility.IsInput)
            return;

        var offset = Vector2.zero;
        for (var i = 0; i < 4; i++)
            if (Input.GetKey(keys[i]))
                offset += Constants.Directions[i];

        offset *= scale;
        if (offset != Vector2.zero)
        {
            tween
                .ChangeValues
                (
                    CameraManager.Position,
                    EditorGrid.Clamp(CameraManager.Position + offset)
                )
                .Restart();
        }
    }
}