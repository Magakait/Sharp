using System;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

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
    private string root;

    public void List(string category)
    {
        this.category = category;
        root = Constants.CollectionRoot + category;

        dropdownTitle.ClearOptions();
        foreach (var option in new DirectoryInfo(root)
                .GetDirectories()
                .OrderBy(d => d.CreationTime)
                .Reverse()
                .Select(d => new Dropdown.OptionData(d.Name)))
            dropdownTitle.options.Add(option);
        dropdownTitle.RefreshShownValue();

        if (dropdownTitle.options.Count > 0)
        {
            var selected = File.ReadAllText(root + "/Selected.txt");
            dropdownTitle.value = dropdownTitle.options.FindIndex(o => o.text == selected);
            dropdownTitle.onValueChanged.Invoke(dropdownTitle.value);
        }
        else
            onEmptyList.Invoke();
    }

    public void Load(string collection)
    {
        if (string.IsNullOrEmpty(collection))
            collection = dropdownTitle.captionText.text;
        File.WriteAllText(root + "/Selected.txt", collection);

        var collectionPath = root + "/" + collection + "/";
        var metaPath = root + "." + collection + ".json";

        if (!File.Exists(metaPath))
            File.Copy(Constants.EditorRoot + "Meta.json", metaPath);

        meta.Load(metaPath);
        info.Load(collectionPath + "Info.json");
        level.Load(collectionPath + "Map.#");
    }

    public void Connect()
    {
        var entrances = LevelManager.instances
            .Select(i => i.GetComponent<EntranceObject>())
            .Where(i => i);

        var focus = entrances.FirstOrDefault(e => e.Threshold == 0);
        if (focus)
            CameraManager.Position = focus.transform.position;
            
        foreach (var entrance in entrances.Where(e => e.Connections != null))
            foreach (var connection in entrance.Connections.Split('\r', '\n'))
            {
                var target = entrances.FirstOrDefault(e => e.Level.ToLower() == connection.ToLower());
                if (target)
                    entrance.Connect(target);
            }

        var valid = (float)entrances.Count(e => e.gameObject.activeSelf);
        meta["progress"] = valid > 0 ? entrances.Count(e => e.Passed) / valid : 1;

        meta["editable"] = category == "Local";
        meta.Save();
    }

    public void Create()
    {
        var path = EngineUtility.NextFile(root, "Collection");
        Directory.CreateDirectory(path);

        foreach (var file in Directory.GetFiles(Constants.EditorRoot + "Collection"))
            File.Copy(file, path + "/" + Path.GetFileName(file));

        Load(Path.GetFileName(path));
    }
}