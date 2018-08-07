using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraPainter : BasePainter<Camera>
{
    public override void Refresh() =>
        component.backgroundColor = Variable.Value;
}