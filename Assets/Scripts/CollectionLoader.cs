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
        this.category = category;
        var index = dropdownTitle.value;

        dropdownTitle.ClearOptions();
        if (Directory.Exists(Constants.CollectionRoot + category))
            foreach (var option in new DirectoryInfo(Constants.CollectionRoot + category)
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

    public void Load(string collection)
    {
        if (string.IsNullOrEmpty(collection))
            collection = dropdownTitle.captionText.text;

        var collectionPath = Constants.CollectionRoot + category + "/" + collection + "/";
        var metaPath = Constants.CollectionRoot + category + "." + collection + ".json";

        if (!File.Exists(metaPath))
            File.Copy(Constants.EditorRoot + "Meta.json", metaPath);

        meta.Load(metaPath);
        info.Load(collectionPath + "Info.json");
        level.Load(collectionPath + "Map.#");

        LevelManager.LoadLevel(level);
        Process();
    }

    private void Process()
    {
        var entrances = LevelManager.instances
            .Select(i => i.GetComponent<EntranceObject>())
            .Where(i => i);

        var passed = meta["passed"].Select(t => ((string)t).ToLower());
        foreach (var entrance in entrances)
            if (passed.Contains(entrance.Level.ToLower()))
                entrance.Pass();

        var valid = (float)entrances.Count(e => e.Valid);
        meta["progress"] = valid > 0 ? entrances.Count(e => e.Passed) / valid : 1;

        meta["editable"] = category == "Local";
        meta.Save();
    }

    public void Create()
    {
        var path = EngineUtility.NextFile(Constants.CollectionRoot + category, "Collection");
        Directory.CreateDirectory(path);

        foreach (var file in Directory.GetFiles(Constants.EditorRoot + "Collection"))
            File.Copy(file, path + "/" + Path.GetFileName(file));

        Load(Path.GetFileName(path));
    }
}