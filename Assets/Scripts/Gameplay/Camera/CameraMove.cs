using UnityEngine;

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

        Vector2 offset = Vector2.zero;
        for (int i = 0; i < 4; i++)
            if (Input.GetKey(keys[i]))
                offset += Constants.Directions[i];

        if (offset != Vector2.zero)
            CameraManager.Position = EditorGrid.Clamp(CameraManager.Position + scale * offset);
    }
}