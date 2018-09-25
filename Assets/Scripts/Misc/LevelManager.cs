using System;
using System.Collections.Generic;

using UnityEngine;

using Newtonsoft.Json.Linq;

[CreateAssetMenu]
public class LevelManager : ScriptableObject
{
    [SerializeField]
    private SerializableObject[] source;

    public static SerializableObject Source(int id) => main.source[id];

    private static LevelManager main;

    public static List<SerializableObject> Instances { get; private set; } = new List<SerializableObject>();

    public void OnEnable()
    {
        main = this;
        Array.Sort(source, (a, b) => a.Id.CompareTo(b.Id));
    }

    #region level management

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

        foreach (var token in CollectionManager.Level.Root)
        {
            var instance = AddInstance((int)token["id"], token["position"].ToVector());
            try
            {
                instance.Deserialize(token["properties"]);
            }
            catch { }
        }
    }

    #endregion

    #region instance management

    public static SerializableObject AddInstance(int id, Vector2 position, bool save = false)
    {
        var instance = Instantiate(Source(id), position, Quaternion.identity);
        instance.name = Source(id).name;
        Instances.Add(instance);

        if (save)
        {
            var data = SerializeInstance(instance);
            ((JArray)CollectionManager.Level.Root).Add(data);

            instance.Deserialize(data["properties"]);
            instance.enabled = false;

            CollectionManager.Level.Save();
        }

        return instance;
    }

    public static void RemoveInstance(SerializableObject instance)
    {
        ((JArray)CollectionManager.Level.Root)[Instances.IndexOf(instance)].Remove();
        CollectionManager.Level.Save();

        Instances.Remove(instance);
        Destroy(instance.gameObject);
    }

    public static void CopyInstance(SerializableObject from, SerializableObject to)
    {
        var properties = new JObject();
        from.Serialize(properties);
        to.Deserialize(properties);

        UpdateInstance(to);
        CollectionManager.Level.Save();
    }

    public static void UpdateInstance(SerializableObject instance)
    {
        ((JArray)CollectionManager.Level.Root)[Instances.IndexOf(instance)].Replace(SerializeInstance(instance));
        CollectionManager.Level.Save();
    }

    private static JToken SerializeInstance(SerializableObject instance)
    {
        var properties = new JObject();
        instance.Serialize(properties);

        return new JObject
        {
            ["id"] = instance.Id,
            ["position"] = instance.transform.position.ToJToken(),
            ["properties"] = properties
        };
    }

    #endregion
}