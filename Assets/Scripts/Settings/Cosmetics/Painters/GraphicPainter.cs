using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphics))]
[AddComponentMenu("Painters/Graphic Painter")]
public class GraphicPainter : BaseCosmetic<ColorVariable>
{
    public override void Refresh()
    {
        Graphic graphic = GetComponent<Graphic>();
        graphic.color = Variable.Value.Fade(graphic.color.a);
    }
}