using System;
using UnityEngine;

namespace AlKaitagi.SharpUI
{
    public class DecimalSlider : BaseSlider
    {
        [SerializeField, Range(0, 2)]
        private int decimals;
        public int Decimals
        {
            get => decimals;
            set => decimals = value;
        }

        protected override float Number(float value) =>
            (float)Math.Round(value, Decimals);

        protected override string Text(float value) =>
            value.ToString();
    }
}
