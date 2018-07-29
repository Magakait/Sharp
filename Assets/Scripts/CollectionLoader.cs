using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Newtonsoft.Json.Linq;

public class CollectionLoader : MonoBehaviour
{
    public Dropdown dropdownTitle;

    [Space(10)]
    public JsonFile meta;
    public JsonFile level;

    public string Category { get; set; }

    public void List()
    {
        int index = dropdownTitle.value;

        dropdownTitle.ClearOptions();
        foreach (var option in new DirectoryInfo(Constants.CollectionsRoot + Category)
                .GetDirectories()
                .OrderBy(d => d.CreationTime)
                .Reverse()
                .Select(d => new Dropdown.OptionData(d.Name)))
            dropdownTitle.options.Add(option);
        dropdownTitle.RefreshShownValue();

        if (dropdownTitle.options.Count > 0)
        {
            dropdownTitle.value = index;
            dropdownTitle.onValueChanged.Invoke(dropdownTitle.value);
        }
    }

    public void Load()
    {
        var path = Constants.CollectionsRoot + Category + dropdownTitle.captionText.text + "/";
        meta.Load(path + "Meta.json");
        level.Load(path + "Map.#");

        LevelManager.Main.LoadLevel(level);
        var passed = (JArray)meta["passed"];
        var entrances = FindObjectsOfType<EntranceObject>();
        foreach (var entrance in entrances)
            if (passed.Contains(entrance.Level))
                entrance.Passed = true;

        meta["progress"] = (float)passed.Count / entrances.Length;
        meta["editable"] = Category == "Local/";
    }

    public void Create()
    {
        var path = EngineUtility.NextFile(Constants.CollectionsRoot + "Local/", "Collection", string.Empty) + "/";
        Directory.CreateDirectory(path);

        File.Copy(Constants.EditorRoot + "Meta.json", path + "Meta.json");
        File.WriteAllText(path + "Map.#", "{}");
        File.Copy(Constants.EditorRoot + "Level.#", path + "Level 1.#");

        meta.Load(path + "Meta.json");
    }
}