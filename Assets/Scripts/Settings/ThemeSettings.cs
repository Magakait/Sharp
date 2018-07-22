using System.IO;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ThemeSettings : MonoBehaviour
{
    public Dropdown dropdownThemes;
    public InputField inputName;
    public CaptionDisplay caption;

    [Space(10)]
    public StringLoader stringLoader;
    public Button deleteButton;

    [Space(10)]
    public JsonFile themeFile;

    public ColorLoader ColorLoader { get; set; }

    public void Color(Color color)
    {
        ColorLoader.Save(color);
        ColorLoader.Load();
    }

    public void List(string name)
    {
        List<string> list = Directory
            .GetFiles(Constants.ThemeRoot, "*.json")
            .Select(i => Path.GetFileNameWithoutExtension(i))
            .ToList();

        deleteButton.interactable = list.Count > 1;

        dropdownThemes.ClearOptions();
        dropdownThemes.AddOptions(list);
        dropdownThemes.RefreshShownValue();

        dropdownThemes.value = list.IndexOf(name);
        dropdownThemes.onValueChanged.Invoke(dropdownThemes.value);
    }

    public void Load()
    {
        string name = dropdownThemes.captionText.text;
        themeFile.Load(Constants.ThemeRoot + name + ".json");
        inputName.text = name;

        stringLoader.Save(name);
    }

    public void Create()
    {
        themeFile.SaveTo(EngineUtility.NextFile(Constants.ThemeRoot, "Theme", ".json"));
        List(themeFile.FileNameWithoutExtension);
    }

    public void Rename(string name)
    {
        string path = Constants.ThemeRoot + name + ".json";

        if (File.Exists(path))
        {
            inputName.text = themeFile.FileNameWithoutExtension;
            caption.Show("This name is already taken.");
        }
        else
        {
            themeFile.Rename(path);
            List(name);
        }
    }

    public void RefreshPainters() =>
        BaseCosmetic<ColorVariable>.RefreshAll();
}