﻿using UnityEngine;
using Sharp.Core.Events;
using Newtonsoft.Json.Linq;

namespace Sharp.Core.JSONLoaders
{
    [AddComponentMenu("Loaders/Bool Loader")]
    public class BoolLoader : BaseLoader<bool>
    {
        [Space(10)]
        public BoolEvent onLoad;

        protected override void Read(JToken value) =>
            onLoad.Invoke((bool)value);

        protected override JToken Write(bool value) =>
            value;
    }
}
