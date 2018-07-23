using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Newtonsoft.Json.Linq;

public class CollectionLoader : MonoBehaviour
{
    public Dropdown dropdownCollections;
    public Transform transformLevels;
    public Button baseButton;

    [Space(10)]
    public JsonFile collectionFile;
    public JsonFile levelFile;

    private void Start() =>
        List();

    public void List()
    {
        int index = dropdownCollections.value;

        dropdownCollections.ClearOptions();
        foreach (string folder in Directory
                .GetDirectories(Constants.CollectionRoot)
                .Select(i => Path.GetFileName(i)))
            dropdownCollections.options.Add(new Dropdown.OptionData(folder));

        if (dropdownCollections.options.Count > 0)
        {
            dropdownCollections.RefreshShownValue();
            dropdownCollections.value = index;
            dropdownCollections.onValueChanged.Invoke(dropdownCollections.value);
        }
    }

    public void Load()
    {
        transformLevels.Clear();

        collectionFile.Load(Constants.CollectionRoot + dropdownCollections.captionText.text + "/Meta.json");

        JArray levels = (JArray)collectionFile["levels"];
        int current = (int)collectionFile["current"];

        collectionFile["progress"] = (float)current / levels.Count;
        collectionFile["editable"] = (int)collectionFile["progress"] == 1;

        foreach (string item in levels
            .Select(i => (string)i)
            .Take(current + 1))
        {
            Button button = Instantiate(baseButton, transformLevels);

            Text text = button.transform.GetChild(0).GetComponent<Text>();
            text.text = item;
            text.alignment = TextAnchor.MiddleCenter;

            button.onClick.AddListener(() => Open(item));
        }
    }

    private void Open(string name)
    {
        levelFile.Load($"{collectionFile.Directory}/{name}.#");
        SceneManager.LoadScene("Play");
    }

    public void Create()
    {
        string path = EngineUtility.NextFile(Constants.CollectionRoot, "Collection", string.Empty);
        Directory.CreateDirectory(path);

        File.Copy(Constants.EditorRoot + "Meta.json", path + "/Meta.json");
        File.Copy(Constants.EditorRoot + "Level.#", path + "/Level 1.#");

        collectionFile.Load(path + "/Meta.json");
    }
}