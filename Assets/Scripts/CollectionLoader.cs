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

    [Space(10)]
    public VoidEvent onEmptyList;

    public string Category { get; set; }

    public void List()
    {
        var index = dropdownTitle.value;

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
        else
            onEmptyList.Invoke();
    }

    public void Load()
    {
        var path = Constants.CollectionsRoot + Category + dropdownTitle.captionText.text + "/";
        meta.Load(path + "Meta.json");
        level.Load(path + "Map.#");

        LevelManager.Main.UnloadLevel();
        LevelManager.Main.LoadLevel(level);

        var passed = meta["passed"].Select(t => (string)t);
        foreach (var entrance in EntranceObject.instances)
            if (passed.Contains(entrance.Level))
                entrance.Pass();

        var last = EntranceObject.instances.FirstOrDefault(e => e.Open && !e.Passed);
        CameraManager.Position = 
            (last ? last : EntranceObject.instances[EntranceObject.instances.Count - 1]).transform.position;

        meta["progress"] = (float)passed.Count() / EntranceObject.instances.Count(e => e.Valid);
        meta["editable"] = Category == "Local/";
        meta.Save();
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