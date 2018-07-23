using UnityEngine;
using UnityEngine.UI;

public class ThemeSettings : MonoBehaviour
{
    public JsonFile file;
    public InputField inputCode;

    public ColorLoader ColorLoader { get; set; }

    private void Awake() =>
        file.Load(Constants.SettingsRoot + "Theme.json");

    public void Defaults() =>
        file.LoadFrom(Constants.SettingsRoot + "Defaults/Theme.json");

    public void SaveColor()
    {
        Color color;
        ColorUtility.TryParseHtmlString("#" + inputCode.text, out color);

        ColorLoader.Save(color);
        RefreshPainters();
    }

    public void DisplayColor(ColorVariable variable) =>
        inputCode.text = ColorUtility.ToHtmlStringRGB(variable);

    public void RefreshPainters()
    {
        foreach (BasePainter painter in FindObjectsOfType<BasePainter>())
            painter.Refresh();
    }
}