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
    private JsonFile meta;
    [SerializeField]
    private JsonFile level;

    [Space(10)]
    [SerializeField]
    private VoidEvent onEmptyList;

    private string category;
    public string Category
    {
        get
        {
            return category;
        }
        set
        {
            category = value + "/";
            List();
        }
    }

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

        LevelManager.LoadLevel(level);

        var entrances = LevelManager.instances
            .Select(i => i.GetComponent<EntranceObject>())
            .Where(i => i);

        var passed = meta["passed"].Select(t => (string)t);
        foreach (var entrance in entrances)
            if (passed.Contains(entrance.Level))
                entrance.Pass();

        meta["progress"] = entrances.Count() > 0 ? (float)passed.Count() / entrances.Count(e => e.Valid) : 1;
        meta["editable"] = Category == "Local/";
        meta.Save();
    }

    public void Create()
    {
        var path = EngineUtility.NextFile(Constants.CollectionsRoot + "Local/", "Collection", string.Empty) + "/";
        Directory.CreateDirectory(path);

        foreach (var file in Directory.GetFiles(Constants.EditorRoot + "Collection"))
            File.Copy(file, path + Path.GetFileName(file));

        meta.Load(path + "Meta.json");
    }
}