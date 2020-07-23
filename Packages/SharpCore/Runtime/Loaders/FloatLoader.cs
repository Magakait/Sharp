using UnityEngine;
using Sharp.Core.Events;
using Newtonsoft.Json.Linq;

namespace Sharp.Core.JSONLoaders
{
    [AddComponentMenu("Loaders/Float Loader")]
    public class FloatLoader : BaseLoader<float>
    {
        [Space(10)]
        public FloatEvent onLoad;

        protected override void Read(JToken value) =>
            onLoad.Invoke((float)value);

        protected override JToken Write(float value) =>
            value;
    }
}
