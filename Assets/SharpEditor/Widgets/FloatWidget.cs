using UnityEngine;
using Sharp.UI;
using Newtonsoft.Json.Linq;

namespace Sharp.Editor.Widgets
{
    public class FloatWidget : BaseWidget
    {
        [Space(10)]
        [SerializeField]
        private DecimalSlider slider;

        protected override void Read(JToken value, JToken attributes)
        {
            slider.Decimals = (int)attributes["decimals"];
            slider.Slider.minValue = (float)attributes["min"];
            slider.Slider.maxValue = (float)attributes["max"];

            slider.Slider.value = (float)value;
        }

        protected override JToken Write() =>
            slider.Slider.value;
    }
}
