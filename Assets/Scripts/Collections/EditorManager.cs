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
        ListLevels(levelFile.FileNameWithoutExtension);

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
        LevelManager.Main.UnloadLevel();
        LevelManager.Main.LoadLevel(levelFile);

        foreach (var instance in FindObjectsOfType<SerializableObject>())
            instance.enabled = false;
    }

    #endregion

    #region collection management

    [Header("Collection")]
    public InputField inputCollection;

    [Space(10)]
    public JsonFile collectionFile;

    private void LoadCollection()
    {
        levels.Clear();
        levels.AddRange(((JArray)collectionFile["levels"]).Select(i => (string)i));

        inputCollection.text = Path.GetFileName(collectionFile.Directory);
    }

    public void RenameCollection(string name)
    {
        string path = Constants.CollectionsRoot + "Local/" + name;

        if (Directory.Exists(path))
            inputCollection.text = Path.GetFileName(collectionFile.Directory);
        else
        {
            Directory.Move(collectionFile.Directory, path);

            collectionFile.Load(path + "/Meta.json");
            levelFile.Load($"{path}/{levelFile.FileNameWithoutExtension}.#");
        }
    }

    #endregion

    #region level management

    [Header("Level")]
    public InputField inputLevel;
    public Toggle baseToggle;
    public ToggleGroup toggleGroup;

    [Space(10)]
    public Button buttonUp;
    public Button buttonDown;

    [Space(10)]
    public JsonFile levelFile;

    [Space(10)]
    public VoidEvent onLevelLoad;

    private static readonly List<string> levels = new List<string>();

    private void LoadLevel(int index)
    {
        levelFile.Load($"{collectionFile.Directory}/{levels[index]}.#");

        inputLevel.text = levelFile.FileNameWithoutExtension;
        DeserializeLevel();

        buttonUp.interactable = index > 0;
        buttonDown.interactable = index < levels.Count - 1;

        onLevelLoad.Invoke();
    }

    private void ListLevels(string name)
    {
        levels.Clear();
        levels.AddRange(((JArray)collectionFile["levels"]).Select(i => (string)i));

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
        string path = EngineUtility.NextFile(collectionFile.Directory + "/", "Level", ".#");
        string name = Path.GetFileNameWithoutExtension(path);

        File.Copy(Constants.EditorRoot + "Level.#", path);
        levels.Add(name);

        ListLevels(name);
    }

    public void CopyLevel()
    {
        levelFile.SaveTo
        (
            EngineUtility.NextFile
            (
                collectionFile.Directory + "/",
                levelFile.FileNameWithoutExtension + " - copy",
                ".#"
            )
        );

        levels.Add(levelFile.FileNameWithoutExtension);
        ListLevels(levelFile.FileNameWithoutExtension);
    }

    public void RenameLevel(string name)
    {
        string path = $"{collectionFile.Directory}/{name}.#";

        if (File.Exists(path))
            inputLevel.text = levelFile.FileNameWithoutExtension;
        else
        {
            levels[levels.IndexOf(levelFile.FileNameWithoutExtension)] = name;
            levelFile.Rename(path);

            ListLevels(name);
        }
    }

    public void DeleteLevel()
    {
        levels.Remove(levelFile.FileNameWithoutExtension);
        levelFile.Delete();

        if (levels.Count > 0)
            ListLevels(string.Empty);
        else
        {
            Directory.Delete(collectionFile.Directory, true);
            EngineUtility.Main.OpenScene("Home");
        }
    }

    public void ShiftOrder(int delta)
    {
        int current = levels.IndexOf(levelFile.FileNameWithoutExtension);
        delta += current;

        string temp = levels[current];
        levels[current] = levels[delta];
        levels[delta] = temp;

        ListLevels(levels[delta]);
    }

    #endregion
}