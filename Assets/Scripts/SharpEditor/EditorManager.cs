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
        inputCollection.text = CollectionManager.Name;
        ListLevels(CollectionManager.Level.ShortName);
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

    #endregion

    #region collection management

    [Header("Collection")]
    public InputField inputCollection;

    public void RenameCollection(string name)
    {
        string path = CollectionManager.FullCategory + name;

        if (Directory.Exists(path))
            inputCollection.text = CollectionManager.Name;
        else
            CollectionManager.MoveTo(path);
    }

    #endregion

    #region level management

    [Header("Level")]
    public InputField inputLevel;
    public Toggle baseToggle;
    public ToggleGroup toggleGroup;

    [Space(10)]
    public BoolEvent onLevelLoad;

    private void LoadLevel(string level)
    {
        CollectionManager.LoadLevel(level);
        foreach (var instance in LevelManager.Instances)
            instance.enabled = false;

        inputLevel.text = CollectionManager.Level.ShortName;
        onLevelLoad.Invoke(CollectionManager.Level.ShortName != "Map");
    }

    private void ListLevels(string name)
    {
        CompleteCollection();
        toggleGroup.transform.Clear();

        foreach (var level in CollectionManager.Levels)
        {
            Toggle toggle = Instantiate(baseToggle, toggleGroup.transform);
            toggle.group = toggleGroup;

            Text text = toggle.transform.GetChild(1).GetComponent<Text>();
            text.text = level;

            toggle.onValueChanged.AddListener(value => { if (value) LoadLevel(level); });
        }

        toggleGroup.transform
            .GetChild(Mathf.Max(0, levels.IndexOf(name)))
            .GetComponent<Toggle>()
            .isOn = true;
    }

    public void AddLevel()
    {
        string path = EngineUtility.NextFile(level.Info.DirectoryName, "Level.#");
        string name = Path.GetFileNameWithoutExtension(path);

        File.Copy(Constants.EditorRoot + "Collection/Level.#", path);
        levels.Add(name);

        ListLevels(name);
    }

    public void CopyLevel()
    {
        level.SaveTo(EngineUtility.NextFile(level.Info.DirectoryName, level.ShortName + " - copy.#"));

        levels.Add(level.ShortName);
        ListLevels(level.ShortName);
    }

    public void RenameLevel(string name)
    {
        string path = $"{level.Info.DirectoryName}/{name}.#";

        if (File.Exists(path))
            inputLevel.text = level.ShortName;
        else
        {
            levels[levels.IndexOf(level.ShortName)] = name;
            level.MoveTo(path);

            ListLevels(name);
        }
    }

    public void DeleteLevel()
    {
        CollectionManager.DeleteLevel();

        if (CollectionManager.Levels.Count > 1)
            ListLevels(CollectionManager.Levels.Last());
        else
        {
            CollectionManager.Delete();
            EngineUtility.Main.LoadScene("Home");
        }
    }

    private void CompleteCollection()
    {
        ((JArray)CollectionManager.Meta["passed"]).ReplaceAll(CollectionManager.Levels);
        CollectionManager.Meta.Save();
    }

    #endregion
}