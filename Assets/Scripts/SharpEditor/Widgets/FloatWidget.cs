using UnityEngine;
using AlKaitagi.SharpUI;
using Newtonsoft.Json.Linq;

public class FloatWidget : BaseWidget
{
    [Space(10)]
    [SerializeField]
    private DecimalSlider slider;

    protected override void Read(JToken value, JToken attributes)
    {
        slider.Decimals = (int)attributes["decimals"];
        slider.Slider.minValue = (float)attributes["range"]["min"];
        slider.Slider.maxValue = (float)attributes["range"]["max"];

        slider.Slider.value = (float)value;
    }

    protected override JToken Write() =>
        slider.Slider.value;
}
