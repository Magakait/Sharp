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
        if (Directory.Exists(Constants.CollectionsRoot + Category))
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
        {
            LevelManager.UnloadLevel();
            onEmptyList.Invoke();
        }
    }

    public void Load()
    {
        var path = Constants.CollectionsRoot + Category + dropdownTitle.captionText.text + "/";
        meta.Load(path + "Meta.json");
        level.Load(path + "Map.#");

        LevelManager.UnloadLevel();
        LevelManager.LoadLevel(level);

        var entrances = LevelManager.instances
            .Select(i => i.GetComponent<EntranceObject>())
            .Where(i => i);

        if (entrances.Count() > 0)
        {
            var passed = meta["passed"].Select(t => (string)t);
            foreach (var entrance in entrances)
                if (passed.Contains(entrance.Level))
                    entrance.Pass();

            meta["progress"] = (float)passed.Count() / entrances.Count(e => e.Valid);
        }
        else
            meta["progress"] = 1;

        meta["editable"] = Category == "Local/";
        meta.Save();
    }

    public void Create()
    {
        var path = EngineUtility.NextFile(Constants.CollectionsRoot + "Local/", "Collection", string.Empty) + "/";
        Directory.CreateDirectory(path);

        File.Copy(Constants.EditorRoot + "Meta.json", path + "Meta.json");
        File.WriteAllText(path + "Map.#", "[]");
        File.Copy(Constants.EditorRoot + "Level.#", path + "Level 1.#");

        meta.Load(path + "Meta.json");
    }
}