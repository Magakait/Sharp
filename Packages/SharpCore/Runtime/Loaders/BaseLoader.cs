using UnityEngine;
using Newtonsoft.Json.Linq;

namespace AlKaitagi.SharpCore.JSONLoaders
{
    public abstract class BaseLoader<T> : MonoBehaviour
    {
        [SerializeField]
        private JsonFile file;
        [SerializeField]
        private string path;

        private static void Restore(JsonFile file, string path)
        {
            var token = file.Root;
            foreach (var key in path.Split('.'))
            {
                if (token[key] == null)
                    token[key] = new JObject();

                token = token[key];
            }
        }

        private void Start()
        {
            try
            {
                Load();
            }
            catch
            {
                Restore(file, path);
                Save(default(T));
                Load();
            }
        }

        public void Load() =>
            Read(file.Root.SelectToken(path));

        public void Save(T value)
        {
            file.Root.SelectToken(path).Replace(Write(value));
            file.Save();
        }

        protected abstract void Read(JToken value);
        protected abstract JToken Write(T value);
    }
}
