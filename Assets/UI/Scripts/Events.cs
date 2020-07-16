using System;
using UnityEngine.Events;

namespace AlKaitagi.SharpUI
{
    [Serializable] public class VoidEvent : UnityEvent { };
    [Serializable] public class BoolEvent : UnityEvent<bool> { };
    [Serializable] public class IntEvent : UnityEvent<int> { };
    [Serializable] public class FloatEvent : UnityEvent<float> { };
    [Serializable] public class StringEvent : UnityEvent<string> { };
}
