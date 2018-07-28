﻿using System.IO;
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
    public JsonFile meta;
    public JsonFile level;

    public void List()
    {
        int index = dropdownCollections.value;

        dropdownCollections.ClearOptions();
        foreach (string folder in Directory
                .GetDirectories(Constants.LocalCollectionsRoot)
                .Select(i => Path.GetFileName(i)))
            dropdownCollections.options.Add(new Dropdown.OptionData(folder));

        if (dropdownCollections.options.Count > 0)
        {
            dropdownCollections.RefreshShownValue();
            dropdownCollections.value = index;
            dropdownCollections.onValueChanged.Invoke(dropdownCollections.value);
        }
    }

    public void Load(string name)
    {
        transformLevels.Clear();

        meta.Load(Constants.LocalCollectionsRoot + dropdownCollections.captionText.text + "/Meta.json");

        JArray levels = (JArray)meta["levels"];
        int current = (int)meta["current"];

        meta["progress"] = (float)current / levels.Count;

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
        level.Load($"{meta.Directory}/{name}.#");
        SceneManager.LoadScene("Play");
    }

    public void Create()
    {
        string path = EngineUtility.NextFile(Constants.LocalCollectionsRoot, "Collection", string.Empty);
        Directory.CreateDirectory(path);

        File.Copy(Constants.EditorRoot + "Meta.json", path + "/Meta.json");
        File.Copy(Constants.EditorRoot + "Level.#", path + "/Level 1.#");

        meta.Load(path + "/Meta.json");
    }
}