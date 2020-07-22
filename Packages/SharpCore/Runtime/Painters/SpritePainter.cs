using UnityEngine;

namespace Sharp.Core.Painters
{
    [RequireComponent(typeof(SpriteRenderer))]
    [AddComponentMenu("Painters/Sprite Painter")]
    public class SpritePainter : BasePainter
    {
        public override void Refresh()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = Fade(Variable.Value, spriteRenderer.color.a);
        }
    }
}
