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
        CollectionManager.Load(root + "/" + collection);
    }

    public void Connect()
    {
        var entrances = LevelManager.instances
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
        CollectionManager.Meta["editable"] = category == "Local";
        CollectionManager.Meta.Save();
    }

    public void Create() => Load(Path.GetFileName(CollectionManager.Create(root)));
}