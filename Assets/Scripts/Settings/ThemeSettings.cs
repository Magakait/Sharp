using UnityEngine;

public class ThemeSettings : MonoBehaviour
{
    public JsonFile file;

    private void Awake() =>
        file.Load(Constants.SettingsRoot + "Theme.json");

    public void Reset() =>
        file.LoadFrom(Constants.SettingsRoot + "Defaults/Theme.json");

    public void RefreshPainters()
    {
        foreach (var painter in FindObjectsOfType<BasePainter>())
            painter.Refresh();
    }
}