using UnityEngine;

using Newtonsoft.Json.Linq;

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