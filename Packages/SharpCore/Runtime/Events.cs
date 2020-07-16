using System;
using UnityEngine;
using UnityEngine.Events;

namespace AlKaitagi.SharpCore.Events
{
    [Serializable] public class VoidEvent : UnityEvent { };
    [Serializable] public class BoolEvent : UnityEvent<bool> { };
    [Serializable] public class IntEvent : UnityEvent<int> { };
    [Serializable] public class FloatEvent : UnityEvent<float> { };
    [Serializable] public class StringEvent : UnityEvent<string> { };
    [Serializable] public class ColorEvent : UnityEvent<Color> { };

}
