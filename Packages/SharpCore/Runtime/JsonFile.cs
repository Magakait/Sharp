using System.IO;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace AlKaitagi.SharpCore
{
    [CreateAssetMenu]
    public class JsonFile : ScriptableObject
    {
        private FileInfo file;

        public string ShortName => Path.GetFileNameWithoutExtension(file.Name);
        public string Name => file.Name;
        public string FullName => file.FullName;

        public JToken Root { get; private set; }
        public JToken this[string path]
        {
            get => Root[path];
            set => Root[path] = value;
        }

        public void Load(string path)
        {
            file = new FileInfo(path);
            Root = JToken.Parse(File.ReadAllText(file.FullName));
        }

        public void LoadFrom(string path)
        {
            File.Copy(path, file.FullName, true);
            Load(file.FullName);
        }

        public void Save() =>
            File.WriteAllText(file.FullName, Root.ToString());

        public void SaveTo(string path)
        {
            Save();
            file.CopyTo(path, true);
            Load(path);
        }

        public void MoveTo(string path) =>
            file.MoveTo(path);

        public void Delete() =>
            file.Delete();
    }
}
