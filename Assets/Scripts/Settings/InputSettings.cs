using System;

using UnityEngine;
using UnityEngine.UI;

class InputSettings : MonoBehaviour
{
    public InputField inputHold;
    public JsonFile file;

    private KeyPicker picker;
    public KeyPicker Picker
    {
        get
        {
            return picker;
        }
        set
        {
            if (value)
                inputHold.Select();
            else if (Picker)
                Picker.onValueChanged.Invoke(Picker.key.Value.ToString());

            picker = value;
            enabled = Picker;
        }
    }

    private void Awake() =>
        file.Load(Constants.SettingsRoot + "Input.json");

    public void Reset() =>
        file.LoadFrom(Constants.SettingsRoot + "Defaults\\Input.json");

    public void Stop() =>
        Picker = null;

    private void Update()
    {
        if (Input.anyKeyDown)
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
                if (Input.GetKeyDown(key))
                {
                    string text = key.ToString();
                    if (!text.Contains("Mouse"))
                    {
                        Picker.SetValue(text);
                        Picker = null;

                        enabled = false;
                        return;
                    }
                }
    }
}