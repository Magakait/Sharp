using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[AddComponentMenu("Skinners/Image Skinner")]
public class ImageSkinner : BaseCosmetic<SpriteVariable>
{
    public override void Refresh()
    {
        GetComponent<Image>().sprite = Variable;
    }
}
