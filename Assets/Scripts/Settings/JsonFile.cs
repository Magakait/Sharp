using System.IO;

using UnityEngine;

using Newtonsoft.Json.Linq;

[CreateAssetMenu]
public class JsonFile : ScriptableObject
{
    private string fileName;
    public string FileName
    {
        get
        {
            return fileName;
        }
        private set
        {
            fileName = value;

            FileNameWithoutExtension = Path.GetFileNameWithoutExtension(FileName);
            Directory = Path.GetDirectoryName(FileName);
        }
    }
    public string FileNameWithoutExtension { get; private set; }
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
        FileName = fileName;
        Refresh();
    }

    public void LoadFrom(string fileName)
    {
        Root = JToken.Parse(File.ReadAllText(fileName));
        Save();
    }

    public void Save() =>
        File.WriteAllText(FileName, Root.ToString());

    public void SaveTo(string fileName)
    {
        File.Copy(FileName, fileName);
        FileName = fileName;
    }

    public void Refresh() =>
        Root = JToken.Parse(File.ReadAllText(FileName));

    public void Rename(string fileName)
    {
        File.Move(FileName, fileName);
        FileName = fileName;
    }

    public void Delete() =>
        File.Delete(FileName);
}