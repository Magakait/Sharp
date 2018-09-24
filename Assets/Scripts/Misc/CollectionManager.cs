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
    public static JsonFile Level => main.level;

    [SerializeField]
    private JsonFile info;
    public static JsonFile Info => main.info;

    [SerializeField]
    private JsonFile meta;
    public static JsonFile Meta => main.meta;

    private static string metaPath => $"{Constants.CollectionRoot}{Category}.{Name}.json";
    private static DirectoryInfo directory;
    private static CollectionManager main;

    public static string Name => directory.Name;
    public static string FullName => directory.FullName;

    public static string Category => directory.Parent.Name;
    public static string FullCategory => directory.Parent.FullName;

    private void OnEnable() => main = this;

    public static void Create(string path)
    {
        Directory.CreateDirectory(path);

        foreach (var file in Directory.GetFiles(Constants.EditorRoot + "Collection"))
            File.Copy(file, path + "/" + Path.GetFileName(file));
    }

    public static void Load(string path)
    {
        directory = new DirectoryInfo(path);

        Level.Load(path + "/Map.#");
        Info.Load(path + "/Info.json");

        var metaPath = CollectionManager.metaPath;
        if (!File.Exists(metaPath))
            File.Copy(Constants.EditorRoot + "Meta.json", metaPath);

        Meta.Load(metaPath);
    }

    public static void MoveTo(string path)
    {
        directory.MoveTo(path);
        Meta.MoveTo(metaPath);
    }

    public static void Delete() => directory.Delete();
}