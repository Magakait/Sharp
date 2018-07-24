using System;

using UnityEngine;

public class DecimalSlider : BaseSlider
{
    [Range(0, 2)]
    [SerializeField]
    private int decimals;
    public int Decimals
    {
        get
        {
            return decimals;
        }
        set
        {
            decimals = value;
        }
    }

    protected override float Number(float value) => 
        (float)Math.Round(value, Decimals);

    protected override string Text(float value) => 
        value.ToString();
}