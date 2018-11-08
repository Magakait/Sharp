using System;

using UnityEngine;

class KeyPicker : MonoBehaviour
{
    public KeyVariable key;
    public StringEvent onValueChanged;

    public void SetValue(string value)
    {
        if (string.IsNullOrEmpty(value))
            onValueChanged.Invoke(key.Value.ToString());
        else
        {
            key.Value = (KeyCode)Enum.Parse(typeof(KeyCode), value);
            onValueChanged.Invoke(value);
        }
    }
}