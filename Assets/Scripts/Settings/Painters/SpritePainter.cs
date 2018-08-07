using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpritePainter : BasePainter<SpriteRenderer>
{
    public override void Refresh() =>
        component.color = Variable.Value.Fade(component.color.a);
}