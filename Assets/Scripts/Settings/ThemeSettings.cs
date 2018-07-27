using System.Linq;
using System.IO;

using UnityEngine;
using UnityEngine.UI;

public class ThemeSettings : MonoBehaviour
{
    public Dropdown dropdown;
    public InputField inputField;
    public Button deleteButton;

    [Space(10)]
    public JsonFile file;
    public StringLoader loader;

    public void List(string theme)
    {
        dropdown.ClearOptions();
        foreach (var option in Directory
                    .GetFiles(Constants.ThemesRoot)
                    .Select(file => Path.GetFileNameWithoutExtension(file))
                    .Select(name => new Dropdown.OptionData(name)))
            dropdown.options.Add(option);
        dropdown.RefreshShownValue();

        deleteButton.interactable = dropdown.options.Count > 1;

        dropdown.value = dropdown.options.FindIndex(option => option.text == theme);
        dropdown.onValueChanged.Invoke(dropdown.value);
    }

    public void Load()
    {
        file.Load(Constants.ThemesRoot + dropdown.captionText.text + ".json");
        loader.Save(file.FileNameWithoutExtension);
        inputField.text = file.FileNameWithoutExtension;
    }

    public void Rename(string name)
    {
        name = Constants.ThemesRoot + name + ".json";
        if (File.Exists(name))
            inputField.text = file.FileNameWithoutExtension;
        else
        {
            file.Rename(name);
            List(file.FileNameWithoutExtension);
        }
    }

    public void Add()
    {
        file.SaveTo(EngineUtility.NextFile(Constants.ThemesRoot, "Theme", ".json"));
        List(file.FileNameWithoutExtension);
    }

    public void Delete()
    {
        file.Delete();
        List(dropdown.options[Mathf.Max(0, dropdown.value - 1)].text);
    }

    public void RefreshPainters()
    {
        foreach (BasePainter painter in FindObjectsOfType<BasePainter>())
            painter.Refresh();
    }
}