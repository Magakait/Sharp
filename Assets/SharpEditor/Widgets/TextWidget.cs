using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

namespace Sharp.Editor.Widgets
{
    public class TextWidget : BaseWidget
    {
        [Space(10)]
        [SerializeField]
        private InputField input;
        [SerializeField]
        private RectTransform panel;

        protected override void Read(JToken value, JToken attributes)
        {
            if ((bool)attributes["multiline"])
            {
                input.lineType = InputField.LineType.MultiLineNewline;
                panel.sizeDelta = new Vector2(panel.sizeDelta.x, 2 * panel.sizeDelta.y);
            }

            input.text = (string)value;
        }

        protected override JToken Write() =>
            input.text;
    }
}
