using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Sharp.Core;
using Newtonsoft.Json.Linq;

[CreateAssetMenu]
public class LevelManager : ScriptableObject
{
    [SerializeField]
    private JsonFile level;
    public static JsonFile Level => main.level;

    [SerializeField]
    private GameObject[] source;
    public static GameObject Source(string name) =>
        main.source.First(s => s.name == name);

    public static List<GameObject> Instances { get; private set; } = new List<GameObject>();

    private static LevelManager main;

    public void OnEnable() =>
        main = this;

    #region level management

    public static void Load(string level) => Level.Load(SetManager.GetLevelFullName(level));

    public static void DestroyAll()
    {
        foreach (var instance in Instances)
            if (instance)
                Destroy(instance.gameObject);

        Instances.Clear();
        System.GC.Collect();
    }

    public static void InstantiateAll()
    {
        DestroyAll();

        foreach (var token in Level.Root)
        {
            var instance = AddInstance((string)token["name"], token["position"].ToVector());
            try
            {
                instance.GetComponent<ISerializable>()?.Deserialize(token["properties"]);
            }
            catch { }
        }
    }

    #endregion

    #region instance management

    public static GameObject AddInstance(string name, Vector2 position, bool save = false)
    {
        var instance = Instantiate(Source(name), position, Quaternion.identity);
        instance.name = name;
        Instances.Add(instance);

        if (save)
        {
            var data = SerializeInstance(instance);
            ((JArray)Level.Root).Add(data);

            instance.GetComponent<ISerializable>()?.Deserialize(data["properties"]);
            if (instance.GetComponent<ISerializable>() is MonoBehaviour mb)
                mb.enabled = false;

            Level.Save();
        }

        return instance;
    }

    public static void RemoveInstance(GameObject instance)
    {
        ((JArray)Level.Root)[Instances.IndexOf(instance)].Remove();
        Level.Save();

        Instances.Remove(instance);
        Destroy(instance.gameObject);
    }

    public static void CopyProperties(GameObject from, GameObject to)
    {
        var properties = new JObject();
        from.GetComponent<ISerializable>()?.Serialize(properties);
        to.GetComponent<ISerializable>()?.Deserialize(properties);

        UpdateInstance(to);
        Level.Save();
    }

    public static void UpdateInstance(GameObject instance)
    {
        ((JArray)Level.Root)[Instances.IndexOf(instance)].Replace(SerializeInstance(instance));
        Level.Save();
    }

    private static JToken SerializeInstance(GameObject instance)
    {
        var properties = new JObject();
        instance.GetComponent<ISerializable>()?.Serialize(properties);

        return new JObject
        {
            ["name"] = instance.name,
            ["position"] = instance.transform.position.ToJToken(),
            ["properties"] = properties
        };
    }

    #endregion
}
