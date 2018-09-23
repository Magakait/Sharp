using System;
using System.Collections.Generic;

using UnityEngine;

using Newtonsoft.Json.Linq;

[CreateAssetMenu]
public class CollectionManager : ScriptableObject
{
    [SerializeField]
    private JsonFile level;
    public static JsonFile Level => Main.level;

    [SerializeField]
    private JsonFile info;
    public static JsonFile Info => Main.info;

    [SerializeField]
    private JsonFile meta;
    public static JsonFile Meta => Main.meta;

    public static CollectionManager Main { get; private set; }

    private void OnEnable() => Main = this;

    public static string Create(string path)
    {
        var path = EngineUtility.NextFile(path, "Collection");
        Directory.CreateDirectory(path);

        foreach (var file in Directory.GetFiles(Constants.EditorRoot + "Collection"))
            File.Copy(file, path + "/" + Path.GetFileName(file));

        return path;
    }

    public static Load(string path)
    {
        level.Load(path + "Map.#");
        info.Load(path + "Info.json");
    }
}