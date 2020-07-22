using UnityEngine;

namespace Sharp.Core.Painters
{
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Painters/Camera Painter")]
    public class CameraPainter : BasePainter
    {
        public override void Refresh() =>
            GetComponent<Camera>().backgroundColor = Variable.Value;
    }
}
