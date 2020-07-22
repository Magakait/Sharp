using UnityEngine;
using AlKaitagi.SharpCore;
using AlKaitagi.SharpCore.Painters;

namespace Sharp.Settings
{
    public class ThemeSettings : MonoBehaviour
    {
        [SerializeField]
        private JsonFile file;

        private void Awake() =>
            file.Load(Constants.SettingsRoot + "Theme.json");

        public void Reset() =>
            file.LoadFrom(Constants.SettingsRoot + "Defaults\\Theme.json");

        public void RefreshPainters()
        {
            foreach (var painter in FindObjectsOfType<BasePainter>())
                painter.Refresh();
        }
    }
}
