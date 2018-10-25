using UnityEngine;

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

        var move = Vector2.zero;
        for (var i = 0; i < 4; i++)
            if (Input.GetKey(keys[i]))
                move += Constants.Directions[i];

        move *= scale;
        if (move != Vector2.zero)
            CameraManager.Move(EditorGrid.Clamp(CameraManager.Position + move), .35f);
    }
}