using UnityEngine;

using DG.Tweening;

public class CameraPan : MonoBehaviour
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
                () => CameraFollow.Position, 
                v => CameraFollow.Position = v, 
                (Vector2)EditorGrid.Clamp(CameraFollow.Position + offset), 
                .35f * Constants.Time
            )
                .SetAutoKill()
                .Play();
        }
    }
}