using UnityEngine;

namespace Sharp.UI
{
    public class PercentageSlider : BaseSlider
    {
        [Range(0, 1)]
        public float min;

        protected override float Number(float value) =>
            Slider.normalizedValue + min * (1 - Slider.normalizedValue);

        protected override string Text(float value) =>
            Mathf.RoundToInt(value * 100) + "%";
    }
}
