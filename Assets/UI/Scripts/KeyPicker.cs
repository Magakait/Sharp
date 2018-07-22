using System;
using UnityEngine;

class KeyPicker : MonoBehaviour
{
    public KeyVariable key;
    public StringEvent onValueChanged;

    public void SetValue(string value)
    {
        key.Value = (KeyCode)Enum.Parse(typeof(KeyCode), value);
        onValueChanged.Invoke(value);
    }
}