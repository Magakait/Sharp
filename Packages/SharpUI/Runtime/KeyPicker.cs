using System;

using UnityEngine;
using UnityEngine.InputSystem;
using Sharp.Core.Events;
using Sharp.Core.Variables;

namespace Sharp.UI
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
