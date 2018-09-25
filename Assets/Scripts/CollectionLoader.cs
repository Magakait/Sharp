using System;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class CollectionLoader : MonoBehaviour
{
    [SerializeField]
    private Dropdown dropdownTitle;
    [SerializeField]
    private VoidEvent onEmptyList;

    private string path;

    public void List(string category)
    {
        path = Constants.CollectionRoot + category;

        dropdownTitle.ClearOptions();
        foreach (var option in new DirectoryInfo(path).GetDirectories()
                .OrderBy(d => d.CreationTime)
                .Reverse()
                .Select(d => new Dropdown.OptionData(d.Name)))
            dropdownTitle.options.Add(option);
        dropdownTitle.RefreshShownValue();

        if (dropdownTitle.options.Count > 0)
        {
            var selected = File.ReadAllText(path + "/Selected.txt");
            dropdownTitle.value = dropdownTitle.options.FindIndex(o => o.text == selected);
            dropdownTitle.onValueChanged.Invoke(dropdownTitle.value);
        }
        else
        {
            onEmptyList.Invoke();
            LevelManager.Unload();
        }
    }

    public void Load(string collection)
    {
        if (string.IsNullOrEmpty(collection))
            collection = dropdownTitle.captionText.text;

        File.WriteAllText(path + "/Selected.txt", collection);
        CollectionManager.Load(path + "/" + collection);
        LevelManager.Load();
    }

    public void Connect()
    {
        var entrances = LevelManager.Instances
            .Select(i => i.GetComponent<EntranceObject>())
            .Where(i => i);

        var focus = entrances.FirstOrDefault(e => e.Threshold == 0);
        if (focus)
            CameraManager.Position = focus.transform.position;

        foreach (var entrance in entrances.Where(e => e.Passed && e.Connections != null))
            foreach (var connection in entrance.Connections.Split('\r', '\n'))
            {
                var target = entrances.FirstOrDefault(e => e.Level == connection);
                if (target)
                    entrance.Connect(target);
            }

        CollectionManager.Meta["completed"] = entrances.All(e => e.Passed);
        CollectionManager.Meta["editable"] = CollectionManager.Category == "Local";
        CollectionManager.Meta.Save();
    }

    public void Create()
    {
        var collection = EngineUtility.NextFile(path, "Collection");
        CollectionManager.Create(collection);
        Load(Path.GetFileName(collection));
    }
}