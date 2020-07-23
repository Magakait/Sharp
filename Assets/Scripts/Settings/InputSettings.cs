using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Sharp.UI;
using Sharp.Core;

namespace Sharp.Settings
{
    public class InputSettings : MonoBehaviour
    {
        public InputField inputHold;
        public JsonFile file;

        private KeyPicker picker;
        public KeyPicker Picker
        {
            get => picker;
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
            if (!Keyboard.current.anyKey.wasPressedThisFrame)
                return;

            foreach (Key key in Enum.GetValues(typeof(Key)))
            {
                if (!Keyboard.current[key].wasPressedThisFrame)
                    continue;

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
}
