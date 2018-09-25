using System;
using System.IO;
using System.Linq;
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

    private static string metaFullName => $"{Constants.CollectionRoot}{Category}.{Name}.json";
    private static DirectoryInfo directory;
    private static CollectionManager main;

    public static string Name => directory.Name;
    public static string FullName => directory.FullName;

    public static string Category => directory.Parent.Name;
    public static string FullCategory => directory.Parent.FullName;

    public static List<string> Levels { get; private set; } = new List<string>();

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

        Levels.Clear();
        Levels.AddRange
        (
            directory.GetFiles("*.#")
                .OrderBy(f => f.CreationTime)
                .Select(f => Path.GetFileNameWithoutExtension(f.Name))
        );

        Levels.Remove("Map");
        Levels.Insert(0, "Map");

        var metaFullName = CollectionManager.metaFullName;
        if (!File.Exists(metaFullName))
            File.Copy(Constants.EditorRoot + "Meta.json", metaFullName);

        Meta.Load(metaFullName);
        Info.Load(FullName + "Info.json");
        LoadLevel("Map");
    }

    public static void MoveTo(string path)
    {
        directory.MoveTo(path);
        Meta.MoveTo(metaFullName);
    }

    public static void Delete() => directory.Delete();

    public static void LoadLevel(string level)
    {
        Level.Load(FullName + level + ".#");
        LevelManager.InstantiateAll();
    }

    public static void DeleteLevel()
    {
        LevelManager.DestroyAll();
        Levels.Remove(Level.ShortName);
        Level.Delete();
    }
}