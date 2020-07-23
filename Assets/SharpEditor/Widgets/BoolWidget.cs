using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

namespace Sharp.Editor.Widgets
{
    public class BoolWidget : BaseWidget
    {
        [Space(10)]
        [SerializeField]
        private Toggle toggle;

        protected override void Read(JToken value, JToken attributes) =>
            toggle.isOn = (bool)value;

        protected override JToken Write() =>
            toggle.isOn;
    }
}
