using UnityEngine;

namespace Sharp.Core.Painters
{
    [RequireComponent(typeof(TrailRenderer))]
    [AddComponentMenu("Painters/Trail Painter")]
    public class TrailPainter : BasePainter
    {
        public override void Refresh()
        {
            var trailRenderer = GetComponent<TrailRenderer>();
            trailRenderer.startColor = Fade(Variable.Value, trailRenderer.startColor.a);
            trailRenderer.endColor = Fade(Variable.Value, trailRenderer.endColor.a);
        }
    }
}
