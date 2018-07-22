using UnityEngine;
using UnityEngine.UI;

using Newtonsoft.Json.Linq;

public class TextWidget : BaseWidget
{
    [Space(10)]
    [SerializeField]
    private InputField input;

    protected override void Read(JToken value, JToken attribute) =>
        input.text = (string)value;

    protected override JToken Write() =>
        input.text;
}