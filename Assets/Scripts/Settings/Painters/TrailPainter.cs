using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class TrailPainter : BasePainter<TrailRenderer>
{
    public override void Refresh()
    {
        component.startColor = Variable.Value.Fade(component.startColor.a);
        component.endColor = Variable.Value.Fade(component.endColor.a);
    }
}