using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Sharp.UI;
using Sharp.Core.Events;
using Newtonsoft.Json.Linq;

public class EditorManager : MonoBehaviour
{
    private void Start()
    {
        inputSet.text = SetManager.Name;

        ListLevels(LevelManager.Level.ShortName);
        IgnoreCollisions(true);
    }

    private void OnDestroy() => IgnoreCollisions(false);

    private void IgnoreCollisions(bool value)
    {
        Physics2D.IgnoreLayerCollision(Constants.UnitLayer, Constants.CellLayer, value);
        Physics2D.IgnoreLayerCollision(Constants.UnitLayer, Constants.FogLayer, value);
    }

    #region set management

    [Header("Set")]
    [SerializeField]
    private InputField inputSet;

    public void RenameSet(string name)
    {
        string path = SetManager.FullCategory + "\\" + name;

        if (Directory.Exists(path))
            inputSet.text = SetManager.Name;
        else
        {
            SetManager.MoveTo(path);
            LevelManager.Load(LevelManager.Level.ShortName);
        }
    }

    private void UpdateSet()
    {
        SetManager.UpdateLevels();

        ((JArray)SetManager.Meta["passed"]).ReplaceAll(SetManager.Levels);
        SetManager.Meta.Save();
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
        UpdateSet();
        toggleGroup.transform.Clear();

        foreach (var level in SetManager.Levels)
        {
            Toggle toggle = Instantiate(baseToggle, toggleGroup.transform);
            toggle.group = toggleGroup;

            Text text = toggle.transform.GetChild(1).GetComponent<Text>();
            text.text = level;

            toggle.onValueChanged.AddListener(value => { if (value) LoadLevel(level); });
        }

        toggleGroup.transform
            .GetChild(Mathf.Max(0, SetManager.Levels.IndexOf(selection)))
            .GetComponent<Toggle>()
            .isOn = true;
    }

    private void LoadLevel(string level)
    {
        LevelManager.Load(level);
        LevelManager.InstantiateAll();

        foreach (var instance in LevelManager.Instances)
            if (instance.GetComponent<ISerializable>() is MonoBehaviour mb)
                mb.enabled = false;

        inputLevel.text = LevelManager.Level.ShortName;
        onLevelLoad.Invoke(LevelManager.Level.ShortName != "Map");
    }

    public void AddLevel()
    {
        string path = UIUtility.NextFile(SetManager.FullName, "Level.#");

        File.Copy(Constants.EditorRoot + "Set\\Level.#", path);
        ListLevels(Path.GetFileNameWithoutExtension(path));
    }

    public void CopyLevel()
    {
        LevelManager.Level.SaveTo(UIUtility.NextFile(SetManager.FullName, LevelManager.Level.ShortName + " - copy.#"));
        ListLevels(LevelManager.Level.ShortName);
    }

    public void RenameLevel(string name)
    {
        if (SetManager.Levels.Contains(name))
            inputLevel.text = LevelManager.Level.ShortName;
        else
        {
            LevelManager.Level.MoveTo(SetManager.GetLevelFullName(name));
            ListLevels(name);
        }
    }

    public void DeleteLevel()
    {
        LevelManager.Level.Delete();

        if (SetManager.Levels.Count > 2)
            ListLevels(string.Empty);
        else
        {
            SetManager.Delete();
            UIUtility.Main.LoadScene("Home");
        }
    }

    #endregion
}
