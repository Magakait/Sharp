using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Sharp.UI;
using Sharp.Core;
using Sharp.Core.Events;
using Sharp.Gameplay;
using Sharp.Camera;

namespace Sharp.Managers
{
    public class SetLoader : MonoBehaviour
    {
        [SerializeField]
        private Dropdown dropdownTitle;
        [SerializeField]
        private VoidEvent onEmptyList;

        private string path;

        public void List(string category)
        {
            path = Constants.SetRoot + category;

            dropdownTitle.ClearOptions();

            dropdownTitle.AddOptions(
                new DirectoryInfo(path).GetDirectories()
                    .OrderBy(d => d.CreationTime)
                    .Reverse()
                    .Select(d => d.Name).ToList());
            dropdownTitle.RefreshShownValue();

            if (dropdownTitle.options.Count > 0)
            {
                var selected = File.ReadAllText(path + "\\Selected.txt");
                var i = dropdownTitle.options.FindIndex(o => o.text == selected);
                dropdownTitle.value = Mathf.Max(i, 0);
                dropdownTitle.onValueChanged.Invoke(dropdownTitle.value);
            }
            else
            {
                onEmptyList.Invoke();
                LevelManager.DestroyAll();
            }
        }

        public void Load(string set)
        {
            if (string.IsNullOrEmpty(set))
                set = dropdownTitle.captionText.text;

            File.WriteAllText(path + "\\Selected.txt", set);

            SetManager.Load(path + "\\" + set);
            LevelManager.Load("Map");
        }

        public void Connect()
        {
            LevelManager.InstantiateAll();
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

            SetManager.Meta["completed"] = entrances.All(e => e.Passed);
            SetManager.Meta["editable"] = SetManager.Category == "Local";
            SetManager.Meta.Save();
        }

        public void Create()
        {
            var set = UIUtility.NextFile(path, "Set");

            SetManager.Create(set);
            Load(Path.GetFileName(set));
        }
    }
}
