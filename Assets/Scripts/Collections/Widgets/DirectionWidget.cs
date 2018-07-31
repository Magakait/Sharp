using UnityEngine;
using UnityEngine.UI;

using Newtonsoft.Json.Linq;

public class DirectionWidget : BaseWidget
{
    [Space(10)]
    [SerializeField]
    private Toggle[] toggles;

    public int Direction { get; set; }

    protected override void Read(JToken value, JToken attributes)
    {
        Direction = (int)value;
        toggles[Direction].isOn = true;
    }

    protected override JToken Write() =>
        Direction;
}