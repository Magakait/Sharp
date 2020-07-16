using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using AlKaitagi.SharpCore;

[CreateAssetMenu]
public class SetManager : ScriptableObject
{
    [SerializeField]
    private JsonFile info;
    public static JsonFile Info => main.info;

    [SerializeField]
    private JsonFile meta;
    public static JsonFile Meta => main.meta;

    public static string GetLevelFullName(string level) => $"{FullName}\\{level}.#";

    public static string Name => directory.Name;
    public static string FullName => directory.FullName;

    public static string Category => directory.Parent.Name;
    public static string FullCategory => directory.Parent.FullName;

    public static List<string> Levels { get; private set; } = new List<string>();

    private static DirectoryInfo directory;
    private static SetManager main;

    public void OnEnable() => main = this;

    public void Update()
    {
        Debug.Log("updating set manager");
    }

    public static void Create(string path)
    {
        Directory.CreateDirectory(path);

        foreach (var file in Directory.GetFiles(Constants.EditorRoot + "Set"))
            File.Copy(file, path + "\\" + Path.GetFileName(file));
    }

    public static void Load(string path)
    {
        directory = new DirectoryInfo(path);
        UpdateFiles();
        UpdateLevels();
    }

    public static void MoveTo(string path)
    {
        directory.MoveTo(path);
        UpdateFiles();
    }

    public static void Delete()
    {
        directory.Delete(true);
        Meta.Delete();
    }

    private static void UpdateFiles()
    {
        var metaFullName = $"{Constants.SetRoot}{Category}.{Name}.json";
        if (!File.Exists(metaFullName))
            File.Copy(Constants.EditorRoot + "Meta.json", metaFullName);

        Meta.Load(metaFullName);
        Info.Load(FullName + "\\Info.json");
    }

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
