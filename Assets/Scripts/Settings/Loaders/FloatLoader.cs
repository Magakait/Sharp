using UnityEngine;

using Newtonsoft.Json.Linq;

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