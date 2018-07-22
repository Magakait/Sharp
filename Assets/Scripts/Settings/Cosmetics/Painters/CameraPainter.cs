using UnityEngine;

[RequireComponent(typeof(Camera))]
[AddComponentMenu("Painters/Camera Painter")]
public class CameraPainter : BaseCosmetic<ColorVariable>
{
    public override void Refresh()
    {
        GetComponent<Camera>().backgroundColor = Variable.Value;
    }
}