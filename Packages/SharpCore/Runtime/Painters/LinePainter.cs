using UnityEngine;

namespace Sharp.Core.Painters
{
    [RequireComponent(typeof(LineRenderer))]
    [AddComponentMenu("Painters/Line Painter")]
    public class LinePainter : BasePainter
    {
        public override void Refresh()
        {
            var lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.startColor = Fade(Variable.Value, lineRenderer.startColor.a);
            lineRenderer.endColor = Fade(Variable.Value, lineRenderer.endColor.a);
        }
    }
}
