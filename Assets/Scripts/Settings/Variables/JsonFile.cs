using System.IO;

using UnityEngine;

using Newtonsoft.Json.Linq;

[CreateAssetMenu]
public class JsonFile : ScriptableObject
{
    private FileInfo info;
    public FileInfo Info
    {
        get
        {
            return info;
        }
        set
        {
            info = value;
            Root = JToken.Parse(File.ReadAllText(Info.FullName));
        }
    }

    public string ShortName => Path.GetFileNameWithoutExtension(Info.Name);

    public JToken Root { get; private set; }
    public JToken this[string path]
    {
        get
        {
            return Root[path];
        }
        set
        {
            Root[path] = value;
        }
    }

    public void Load(string fileName) => Info = new FileInfo(fileName);

    public void LoadFrom(string fileName)
    {
        File.Copy(fileName, Info.FullName, true);
        Load(Info.FullName);
    }

    public void Save() => File.WriteAllText(Info.FullName, Root.ToString());

    public void SaveTo(string fileName)
    {
        Save();
        Info.CopyTo(fileName, true);
        Load(fileName);
    }

    public void MoveTo(string fileName) => Info.MoveTo(fileName);

    public void Delete() => Info.Delete();
}