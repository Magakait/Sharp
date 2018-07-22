using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[AddComponentMenu("Skinners/Sprite Skinner")]
public class SpriteSkinner : BaseCosmetic<SpriteVariable>
{
    public override void Refresh()
    {
        GetComponent<SpriteRenderer>().sprite = Variable;
    }
}
