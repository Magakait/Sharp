using System;
using System.IO;
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

    public static DirectoryInfo Directory { get; private set; }

    private void OnEnable() => Main = this;

    public static string Create(string path)
    {
        path = EngineUtility.NextFile(path, "Collection");
        System.IO.Directory.CreateDirectory(path);

        foreach (var file in System.IO.Directory.GetFiles(Constants.EditorRoot + "Collection"))
            File.Copy(file, path + "/" + Path.GetFileName(file));

        return path;
    }

    public static void Load(string path)
    {
        Directory = new DirectoryInfo(path);

        Level.Load(path + "/Map.#");
        Info.Load(path + "/Info.json");

        var metaPath = Constants.CollectionRoot + Directory.Parent.Name + "." + Directory.Name + ".json";
        if (!File.Exists(metaPath))
            File.Copy(Constants.EditorRoot + "Meta.json", metaPath);

        Meta.Load(metaPath);
    }

    public static void Delete() => Directory.Delete();
}