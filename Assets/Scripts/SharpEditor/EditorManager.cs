using System.IO;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Newtonsoft.Json.Linq;

public class EditorManager : MonoBehaviour
{
    private void Start()
    {
        LoadCollection();
        ListLevels(level.Name);

        IgnoreCollisions(true);
    }

    private void OnDestroy() =>
        IgnoreCollisions(false);

    #region engine management

    private void IgnoreCollisions(bool value)
    {
        Physics2D.IgnoreLayerCollision(Constants.UnitLayer, Constants.CellLayer, value);
        Physics2D.IgnoreLayerCollision(Constants.UnitLayer, Constants.FogLayer, value);
    }

    public void DeserializeLevel()
    {
        LevelManager.LoadLevel(level);

        foreach (var instance in FindObjectsOfType<SerializableObject>())
            instance.enabled = false;
    }

    #endregion

    #region collection management

    [Header("Collection")]
    public InputField inputCollection;

    [Space(10)]
    public JsonFile meta;

    private void LoadCollection()
    {
        levels.Clear();
        levels.AddRange
        (
            new DirectoryInfo(level.Directory)
                .GetFiles("*.#")
                .OrderBy(f => f.CreationTime)
                .Select(f => Path.GetFileNameWithoutExtension(f.Name))
        );
        levels.Remove("Map");
        levels.Insert(0, "Map");

        inputCollection.text = Path.GetFileName(level.Directory);
    }

    public void RenameCollection(string name)
    {
        string path = Constants.CollectionsRoot + "Local/" + name;

        if (Directory.Exists(path))
            inputCollection.text = Path.GetFileName(level.Directory);
        else
        {
            Directory.Move(level.Directory, path);

            meta.Rename($"{new DirectoryInfo(level.Directory).Parent.Name}.{name}.json");
            level.Load($"{path}/{level.Name}.#");
        }
    }

    #endregion

    #region level management

    [Header("Level")]
    public InputField inputLevel;
    public Toggle baseToggle;
    public ToggleGroup toggleGroup;

    [Space(10)]
    public JsonFile level;

    [Space(10)]
    public BoolEvent onLevelLoad;

    private static readonly List<string> levels = new List<string>();

    private void LoadLevel(int index)
    {
        level.Load($"{level.Directory}/{levels[index]}.#");

        inputLevel.text = level.Name;
        DeserializeLevel();

        onLevelLoad.Invoke(level.Name != "Map");
    }

    private void ListLevels(string name)
    {
        toggleGroup.transform.Clear();

        for (int i = 0; i < levels.Count; i++)
        {
            Toggle toggle = Instantiate(baseToggle, toggleGroup.transform);
            toggle.group = toggleGroup;

            Text text = toggle.transform.GetChild(1).GetComponent<Text>();
            text.text = levels[i];

            int index = i;
            toggle.onValueChanged.AddListener(value => { if (value) LoadLevel(index); });
        }

        toggleGroup.transform
            .GetChild(Mathf.Max(0, levels.IndexOf(name)))
            .GetComponent<Toggle>()
            .isOn = true;
    }

    public void AddLevel()
    {
        string path = EngineUtility.NextFile(level.Directory + "/", "Level", ".#");
        string name = Path.GetFileNameWithoutExtension(path);

        File.Copy(Constants.EditorRoot + "Collection/Level.#", path);
        levels.Add(name);

        ListLevels(name);
    }

    public void CopyLevel()
    {
        level.SaveTo
        (
            EngineUtility.NextFile
            (
                level.Directory + "/",
                level.Name + " - copy",
                ".#"
            )
        );

        levels.Add(level.Name);
        ListLevels(level.Name);
    }

    public void RenameLevel(string name)
    {
        string path = $"{level.Directory}/{name}.#";

        if (File.Exists(path))
            inputLevel.text = level.Name;
        else
        {


            levels[levels.IndexOf(level.Name)] = name;
            level.Rename(path);

            ListLevels(name);
        }
    }

    public void DeleteLevel()
    {
        levels.Remove(level.Name);
        level.Delete();

        if (levels.Count > 1)
            ListLevels(levels[levels.Count - 1]);
        else
        {
            Directory.Delete(level.Directory, true);
            meta.Delete();

            EngineUtility.Main.LoadScene("Home");
        }
    }

    private void PassLevels()
    {
        meta.Save();
    }

    #endregion
}