using UnityEngine;
using AlKaitagi.SharpCore.Events;
using Newtonsoft.Json.Linq;

namespace AlKaitagi.SharpCore.JSONLoaders
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
