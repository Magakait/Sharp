using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LinePainter : BasePainter<LineRenderer>
{
    public override void Refresh()
    {
        component.startColor = Variable.Value.Fade(component.startColor.a);
        component.endColor = Variable.Value.Fade(component.endColor.a);
    }
}