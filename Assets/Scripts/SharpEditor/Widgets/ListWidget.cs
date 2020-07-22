using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class ListWidget : BaseWidget
{
    [Space(10)]
    [SerializeField]
    private Dropdown dropdown;

    protected override void Read(JToken value, JToken attributes)
    {
        dropdown.ClearOptions();

        foreach (JToken option in attributes["options"])
            dropdown.options.Add(new Dropdown.OptionData((string)option));

        dropdown.RefreshShownValue();
        dropdown.value = (int)value;
    }

    protected override JToken Write() =>
        dropdown.value;
}
