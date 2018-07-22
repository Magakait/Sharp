using UnityEngine;

using Newtonsoft.Json.Linq;

public abstract class BaseLoader<T> : MonoBehaviour
{
    [SerializeField]
    private JsonFile file;
    [SerializeField]
    private string path;

    private void Start() =>
        Load();

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