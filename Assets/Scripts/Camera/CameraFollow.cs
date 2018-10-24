using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target { get; set; }

    private void Update()
    {
        if (Target)
            CameraManager.Move(Target.position);
    }
}