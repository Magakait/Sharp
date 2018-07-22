using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class BackgroundSettings : MonoBehaviour
{
    public Toggle baseToggle;
    public ToggleGroup toggleGroup;

    [Space(10)]
    public StringLoader loader;

    [Space(10)]
    public JsonFile file;
    public LevelManager manager;

    public void List(string name)
    {
        toggleGroup.transform.Clear();

        foreach (string item in Directory
            .GetFiles(Constants.BackgroundRoot, "*.#")
            .Select(i => Path.GetFileNameWithoutExtension(i)))
        {
            Toggle toggle = Instantiate(baseToggle, toggleGroup.transform);
            toggle.group = toggleGroup;

            toggle.transform.GetChild(1).GetComponent<Text>().text = item;
            toggle.onValueChanged.AddListener(value => Load(item));

            if (item == name)
                toggle.isOn = true;
        }
    }

    private void Load(string name)
    {
        loader.Save(name);
        file.Load(Constants.BackgroundRoot + name + ".#");

        manager.UnloadLevel();
        manager.LoadLevel(file);

        Destroy(LevelManager.Main.instances.First(i => i.Id == 0).gameObject);
        Destroy(LevelManager.Main.instances.First(i => i.Id == 1).gameObject);
    }
}