using UnityEngine;

public class ThemeSettings : MonoBehaviour
{
    public JsonFile file;

    private void Awake() =>
        file.Load(Constants.SettingsRoot + "Theme.json");

    public void Defaults() =>
        file.LoadFrom(Constants.SettingsRoot + "Defaults/Theme.json");

    public void RefreshPainters()
    {
        foreach (BasePainter painter in FindObjectsOfType<BasePainter>())
            painter.Refresh();
    }
}