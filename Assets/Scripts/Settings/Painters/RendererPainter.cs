using UnityEngine;

public class RendererPainter : BasePainter
{
    [SerializeField]
    private new Renderer renderer;

    public override void Refresh() =>
        renderer.material.color = Variable.Value.Fade(renderer.material.color.a);
}