using UnityEngine;

public class LayerToggle : MonoBehaviour
{
    private bool visible;
    public bool Visible
    {
        get
        {
            return visible;
        }
        set
        {
            visible = value;

            if (Visible)
                CameraMain.Camera.cullingMask |= mask;
            else
                CameraMain.Camera.cullingMask &= ~mask;
        }
    }

    [SerializeField]
    private LayerMask mask;

    private void Awake() => 
        Visible = true;

    private void OnDestroy() => 
        Awake();
}