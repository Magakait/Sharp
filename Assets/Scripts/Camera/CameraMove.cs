using UnityEngine;

using DG.Tweening;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private float scale;
    [SerializeField]
    private KeyVariable[] keys;

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
            DOTween.To
            (
                () => CameraManager.Position, 
                v => CameraManager.Position = v, 
                (Vector2)EditorGrid.Clamp(CameraManager.Position + offset), 
                .25f * Constants.Time
            )
                .SetAutoKill()
                .Play();
        }
    }
}