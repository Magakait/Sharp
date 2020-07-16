using System;

using UnityEngine;
using UnityEngine.InputSystem;
using AlKaitagi.SharpCore.Events;
using AlKaitagi.SharpCore.Variables;

namespace AlKaitagi.SharpUI
{
    public class KeyPicker : MonoBehaviour
    {
        public KeyVariable key;
        public StringEvent onValueChanged;

        public void SetValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                onValueChanged.Invoke(key.Value.ToString());
            else
            {
                key.Value = (Key)Enum.Parse(typeof(Key), value);
                onValueChanged.Invoke(value);
            }
        }
    }
}
