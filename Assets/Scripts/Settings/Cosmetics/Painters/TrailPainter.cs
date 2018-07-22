using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
[AddComponentMenu("Painters/Trail Painter")]
public class TrailPainter : BaseCosmetic<ColorVariable>
{
    public override void Refresh()
    {
        TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.startColor = Variable.Value.Fade(trailRenderer.startColor.a);
        trailRenderer.endColor = Variable.Value.Fade(trailRenderer.endColor.a);
    }
}