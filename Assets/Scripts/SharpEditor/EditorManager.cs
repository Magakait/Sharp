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

        ListLevels(LevelManager.Level.ShortName);
        IgnoreCollisions(true);
    }

    private void OnDestroy() => IgnoreCollisions(false);

    private void IgnoreCollisions(bool value)
    {
        Physics2D.IgnoreLayerCollision(Constants.UnitLayer, Constants.CellLayer, value);
        Physics2D.IgnoreLayerCollision(Constants.UnitLayer, Constants.FogLayer, value);
    }

    #region collection management

    [Header("Collection")]
    [SerializeField]
    private InputField inputCollection;

    public void RenameCollection(string name)
    {
        string path = CollectionManager.FullCategory + "\\" + name;

        if (Directory.Exists(path))
            inputCollection.text = CollectionManager.Name;
        else
            CollectionManager.MoveTo(path);
    }

    private void UpdateCollection()
    {
        CollectionManager.UpdateLevels();

        ((JArray)CollectionManager.Meta["passed"]).ReplaceAll(CollectionManager.Levels);
        CollectionManager.Meta.Save();
    }

    #endregion

    #region level management

    [Header("Level")]
    [SerializeField]
    private InputField inputLevel;
    [SerializeField]
    private Toggle baseToggle;
    [SerializeField]
    private ToggleGroup toggleGroup;

    [Space(10)]
    [SerializeField]
    private BoolEvent onLevelLoad;

    private void ListLevels(string selection)
    {
        UpdateCollection();
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
            .GetChild(Mathf.Max(0, CollectionManager.Levels.IndexOf(selection)))
            .GetComponent<Toggle>()
            .isOn = true;
    }

    private void LoadLevel(string level)
    {
        LevelManager.Load(level);
        LevelManager.InstantiateAll();

        foreach (var instance in LevelManager.Instances)
            instance.enabled = false;

        inputLevel.text = LevelManager.Level.ShortName;
        onLevelLoad.Invoke(LevelManager.Level.ShortName != "Map");
    }

    public void AddLevel()
    {
        string path = EngineUtility.NextFile(CollectionManager.FullName, "Level.#");

        File.Copy(Constants.EditorRoot + "Collection\\Level.#", path);
        ListLevels(Path.GetFileNameWithoutExtension(path));
    }

    public void CopyLevel()
    {
        LevelManager.Level.SaveTo(EngineUtility.NextFile(CollectionManager.FullName, LevelManager.Level.ShortName + " - copy.#"));
        ListLevels(LevelManager.Level.ShortName);
    }

    public void RenameLevel(string name)
    {
        if (CollectionManager.Levels.Contains(name))
            inputLevel.text = LevelManager.Level.ShortName;
        else
        {
            LevelManager.Level.MoveTo(CollectionManager.GetLevelFullName(name));
            ListLevels(name);
        }
    }

    public void DeleteLevel()
    {
        LevelManager.Level.Delete();

        if (CollectionManager.Levels.Count > 1)
            ListLevels(string.Empty);
        else
        {
            CollectionManager.Delete();
            EngineUtility.Main.LoadScene("Home");
        }
    }

    #endregion
}