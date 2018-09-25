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

        ListLevels(level.ShortName);

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

    private static readonly List<string> levels = new List<string>();

    private void LoadLevel(int index)
    {
        level.Load($"{level.Info.DirectoryName}/{levels[index]}.#");

        inputLevel.text = level.ShortName;

        foreach (var instance in FindObjectsOfType<SerializableObject>())
            instance.enabled = false;

        onLevelLoad.Invoke(CollectionManager.Level.ShortName != "Map");
    }

    private void ListLevels(string name)
    {
        Complete();
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
        levels.Remove(level.ShortName);
        level.Delete();

        if (levels.Count > 1)
            ListLevels(levels[levels.Count - 1]);
        else
        {
            level.Info.Directory.Delete(true);
            meta.Delete();

            EngineUtility.Main.LoadScene("Home");
        }
    }

    private void Complete()
    {
        var passed = (JArray)CollectionManager.Meta["passed"];
        passed.Clear();

        foreach (var level in levels.Skip(1))
            passed.Add(level);

        CollectionManager.Meta.Save();
    }

    #endregion
}