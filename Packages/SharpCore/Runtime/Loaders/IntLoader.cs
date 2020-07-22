using UnityEngine;
using Sharp.Core.Events;
using Newtonsoft.Json.Linq;

namespace Sharp.Core.JSONLoaders
{
    [AddComponentMenu("Loaders/Int Loader")]
    public class IntLoader : BaseLoader<int>
    {
        [Space(10)]
        public IntEvent onLoad;

        protected override void Read(JToken value) =>
            onLoad.Invoke((int)value);

        protected override JToken Write(int value) =>
            value;
    }
}
