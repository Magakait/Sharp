using UnityEngine;
using Sharp.Core.Events;
using Newtonsoft.Json.Linq;

namespace Sharp.Core.JSONLoaders
{
    [AddComponentMenu("Loaders/String Loader")]
    public class StringLoader : BaseLoader<string>
    {
        [Space(10)]
        public StringEvent onLoad;

        protected override void Read(JToken value) =>
            onLoad.Invoke((string)value);

        protected override JToken Write(string value) =>
            value;
    }
}
