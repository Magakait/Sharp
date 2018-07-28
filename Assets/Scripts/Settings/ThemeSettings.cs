using System.Linq;
using System.IO;

using UnityEngine;
using UnityEngine.UI;

public class ThemeSettings : MonoBehaviour
{
    public JsonFile file;

    private void Awake() =>
        file.Load(Constants.SettingsRoot + "Theme.json");

    public void Reset() =>
        file.Load(Constants.SettingsRoot + "Defaults/Theme.json");

    public void RefreshPainters()
    {
        foreach (BasePainter painter in FindObjectsOfType<BasePainter>())
            painter.Refresh();
    }
}