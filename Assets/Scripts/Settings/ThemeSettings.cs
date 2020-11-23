using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sharp.Core;
using Sharp.Core.Events;
using Sharp.Core.Painters;

namespace Sharp.Settings
{
    public class ThemeSettings : MonoBehaviour
    {
        [SerializeField] private JsonFile file;
        [SerializeField] private Dropdown dropdownThemes;

        [SerializeField] BoolEvent onEditable;
        [SerializeField] BoolEvent onMultiple;

        List<string> themes = new List<string>();

        private void Awake()
        {
            themes = Directory
                .GetFiles(Constants.ThemesRoot, "*.json")
                .Select(f => Path.GetFileNameWithoutExtension(f))
                .ToList();
            themes.Remove("Custom");
            themes.Remove("Default");
            themes.Insert(0, "Custom");
            dropdownThemes.AddOptions(themes);

            var index = themes.IndexOf(File.ReadAllText(Constants.ThemesRoot + "Selected.txt"));
            dropdownThemes.value = Mathf.Max(index, 0);
            dropdownThemes.RefreshShownValue();

            onMultiple.Invoke(themes.Count > 1);
        }

        public void LoadTheme(int index)
        {
            var name = themes[index];
            file.Load(Constants.ThemesRoot + name + ".json");
            File.WriteAllText(Constants.ThemesRoot + "Selected.txt", name);
            onEditable.Invoke(name == "Custom");
        }

        public void ResetTheme()
        {
            file.LoadFrom(Constants.ThemesRoot + "Default.json");
            dropdownThemes.onValueChanged.Invoke(dropdownThemes.value);
        }

        public void RefreshPainters()
        {
            foreach (var painter in FindObjectsOfType<BasePainter>())
                painter.Refresh();
        }
    }
}
