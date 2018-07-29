using System.IO;

using UnityEngine;

using Newtonsoft.Json.Linq;

[CreateAssetMenu]
public class JsonFile : ScriptableObject
{
    private string fullName;
    public string FullName
    {
        get
        {
            return fullName;
        }
        private set
        {
            fullName = value;

            Name = Path.GetFileNameWithoutExtension(FullName);
            Directory = Path.GetDirectoryName(FullName);
        }
    }
    public string Name { get; private set; }
    public string Directory { get; private set; }

    public JToken Root { get; set; }
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

    public void Load(string fileName)
    {
        FullName = fileName;
        Refresh();
    }

    public void LoadFrom(string fileName)
    {
        Root = JToken.Parse(File.ReadAllText(fileName));
        Save();
    }

    public void Save() =>
        File.WriteAllText(FullName, Root.ToString());

    public void SaveTo(string fileName)
    {
        File.Copy(FullName, fileName);
        FullName = fileName;
    }

    public void Refresh() =>
        Root = JToken.Parse(File.ReadAllText(FullName));

    public void Rename(string fileName)
    {
        File.Move(FullName, fileName);
        FullName = fileName;
    }

    public void Delete() =>
        File.Delete(FullName);
}