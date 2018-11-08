using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using Newtonsoft.Json.Linq;

[CreateAssetMenu]
public class SetManager : ScriptableObject
{
    [SerializeField]
    private JsonFile info;
    public static JsonFile Info => main.info;

    [SerializeField]
    private JsonFile meta;
    public static JsonFile Meta => main.meta;

    private static string GetMetaFullName => $"{Constants.SetRoot}{Category}.{Name}.json";
    public static string GetLevelFullName(string level) => $"{FullName}\\{level}.#";

    public static string Name => directory.Name;
    public static string FullName => directory.FullName;

    public static string Category => directory.Parent.Name;
    public static string FullCategory => directory.Parent.FullName;

    public static List<string> Levels { get; private set; } = new List<string>();

    private static DirectoryInfo directory;
    private static SetManager main;

    public void OnEnable() => main = this;

    public static void Create(string path)
    {
        Directory.CreateDirectory(path);

        foreach (var file in Directory.GetFiles(Constants.EditorRoot + "Set"))
            File.Copy(file, path + "\\" + Path.GetFileName(file));
    }

    public static void Load(string path)
    {
        directory = new DirectoryInfo(path);

        var metaFullName = SetManager.GetMetaFullName;
        if (!File.Exists(metaFullName))
            File.Copy(Constants.EditorRoot + "Meta.json", metaFullName);

        Meta.Load(metaFullName);
        Info.Load(FullName + "\\Info.json");

        UpdateLevels();
    }

    public static void MoveTo(string path)
    {
        directory.MoveTo(path);
        Meta.MoveTo(GetMetaFullName);
        Load(path);
    }

    public static void Delete() => directory.Delete(true);

    public static void UpdateLevels()
    {
        Levels.Clear();
        Levels.AddRange
        (
            directory.GetFiles("*.#")
                .OrderBy(f => f.CreationTime)
                .Select(f => Path.GetFileNameWithoutExtension(f.Name))
        );

        Levels.Remove("Map");
        Levels.Insert(0, "Map");
    }
}