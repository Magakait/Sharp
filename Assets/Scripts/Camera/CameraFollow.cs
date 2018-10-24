using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    public static Transform Target { get; set; }

    private void Update()
    {
        if (Target)
            CameraManager.Move(Target.position);
    }
}