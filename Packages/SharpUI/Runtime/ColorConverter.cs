using UnityEngine;
using Sharp.Core.Events;

namespace Sharp.UI
{
    public class ColorConverter : MonoBehaviour
    {
        public StringEvent toString;
        public ColorEvent toColor;

        public void ToString(Color value) =>
            toString.Invoke(ColorUtility.ToHtmlStringRGB(value));

        public void ToColor(string value)
        {
            Color color;
            ColorUtility.TryParseHtmlString("#" + value, out color);

            toColor.Invoke(color);
        }
    }
}
