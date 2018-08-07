using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[AddComponentMenu("Painters/Line Painter")]
public class LinePainter : BasePainter
{
    public override void Refresh()
    {
        var lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = Variable.Value.Fade(lineRenderer.startColor.a);
        lineRenderer.endColor = Variable.Value.Fade(lineRenderer.endColor.a);
    }
}