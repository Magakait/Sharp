using System;
using System.Collections.Generic;

using UnityEngine;

using Newtonsoft.Json.Linq;

[CreateAssetMenu]
public class LevelManager : ScriptableObject
{
    [SerializeField]
    private SerializableObject[] source;
    private static LevelManager main;

    public static readonly List<SerializableObject> instances = new List<SerializableObject>();
    private static JsonFile level;

    public void OnEnable()
    {
        main = this;
        Array.Sort(source, (a, b) => a.Id.CompareTo(b.Id));
    }

    public static SerializableObject Source(int id) =>
        main.source[id];

    #region level management

    public static void UnloadLevel()
    {
        foreach (var instance in instances)
            if (instance)
                Destroy(instance.gameObject);

        instances.Clear();
        System.GC.Collect();
    }

    public static void LoadLevel(JsonFile file)
    {
        UnloadLevel();
        level = file;

        foreach (var token in level.Root)
        {
            var instance = AddInstance((int)token["id"], token["position"].ToVector());
            try
            {
                instance.Deserialize(token["properties"]);
            }
            catch { }
        }

        System.GC.Collect();
    }

    #endregion

    #region instance management

    public static SerializableObject AddInstance(int id, Vector2 position, bool save = false)
    {
        var instance = Instantiate(Source(id), position, Quaternion.identity);
        instance.name = Source(id).name;
        instances.Add(instance);

        if (save)
        {
            var data = SerializeInstance(instance);
            ((JArray)level.Root).Add(data);

            instance.Deserialize(data["properties"]);
            instance.enabled = false;

            level.Save();
        }

        return instance;
    }

    public static void RemoveInstance(SerializableObject instance)
    {
        ((JArray)level.Root)[instances.IndexOf(instance)].Remove();
        level.Save();

        instances.Remove(instance);
        Destroy(instance.gameObject);
    }

    public static void CopyInstance(SerializableObject from, SerializableObject to)
    {
        var properties = new JObject();
        from.Serialize(properties);
        to.Deserialize(properties);

        UpdateInstance(to);
        level.Save();
    }

    public static void UpdateInstance(SerializableObject instance)
    {
        ((JArray)level.Root)[instances.IndexOf(instance)].Replace(SerializeInstance(instance));
        level.Save();
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