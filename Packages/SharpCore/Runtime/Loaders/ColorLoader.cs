using UnityEngine;
using AlKaitagi.SharpCore.Events;
using Newtonsoft.Json.Linq;

namespace AlKaitagi.SharpCore.JSONLoaders
{
    [AddComponentMenu("Loaders/Color Loader")]
    public class ColorLoader : BaseLoader<Color>
    {
        [Space(10)]
        public ColorEvent onLoad;

        protected override void Read(JToken value)
        {
            Color color;
            ColorUtility.TryParseHtmlString("#" + (string)value, out color);
            onLoad.Invoke(color);
        }

        protected override JToken Write(Color value) =>
            ColorUtility.ToHtmlStringRGB(value);
    }
}
