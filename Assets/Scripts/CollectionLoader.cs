using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Newtonsoft.Json.Linq;

public class CollectionLoader : MonoBehaviour
{
    [SerializeField]
    private Dropdown dropdownTitle;

    [Space(10)]
    [SerializeField]
    private JsonFile info;
    [SerializeField]
    private JsonFile meta;
    [SerializeField]
    private JsonFile level;

    [Space(10)]
    [SerializeField]
    private VoidEvent onEmptyList;

    private string category;

    public void List(string category)
    {
        this.category = category + "/";
        var index = dropdownTitle.value;

        dropdownTitle.ClearOptions();
        if (Directory.Exists(Constants.CollectionsRoot + category))
            foreach (var option in new DirectoryInfo(Constants.CollectionsRoot + category)
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
        var path = Constants.CollectionsRoot + category + dropdownTitle.captionText.text + "/";
        info.Load(path + "Info.json");
        if (!File.Exists(path + "Meta.json"))
            File.Copy(Constants.EditorRoot + "Collection/Meta.json", path + "Meta.json");
        meta.Load(path + "Meta.json");
        level.Load(path + "Map.#");

        LevelManager.LoadLevel(level);

        var entrances = LevelManager.instances
            .Select(i => i.GetComponent<EntranceObject>())
            .Where(i => i);

        var passed = meta["passed"].Select(t => (string)t);
        foreach (var entrance in entrances)
            if (passed.Contains(entrance.Level))
                entrance.Pass();

        var valid = (float)entrances.Count(e => e.Valid);
        meta["progress"] = valid > 0 ? entrances.Count(e => e.Passed) / valid : 1;

        meta["editable"] = category == "Local/";
        meta.Save();
    }

    public void Create()
    {
        var path = EngineUtility.NextFile(Constants.CollectionsRoot + category, "Collection", string.Empty) + "/";
        Directory.CreateDirectory(path);

        foreach (var file in Directory.GetFiles(Constants.EditorRoot + "Collection"))
            File.Copy(file, path + Path.GetFileName(file));

        info.Load(path + "Info.json");
        meta.Load(path + "Meta.json");
    }
}