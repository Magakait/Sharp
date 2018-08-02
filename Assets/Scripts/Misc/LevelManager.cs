using System;
using System.Collections.Generic;

using UnityEngine;

using Newtonsoft.Json.Linq;

[CreateAssetMenu]
public class LevelManager : ScriptableObject
{
    [SerializeField]
    private SerializableObject[] source;
    public SerializableObject Source(int id) => 
        source[id];

    private readonly List<SerializableObject> instances = new List<SerializableObject>();

    public static LevelManager Main { get; private set; }
    private JsonFile level;

    public void OnEnable()
    {
        Main = this;
        Array.Sort(source, (a, b) => a.Id.CompareTo(b.Id));
    }

    #region level management

    public void UnloadLevel()
    {
        foreach (var instance in instances)
            if (instance)
                Destroy(instance.gameObject);

        instances.Clear();
    }

    public void LoadLevel(JsonFile file)
    {
        level = file;
        instances.Clear();

        foreach (var token in level.Root)
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

    public SerializableObject AddInstance(int id, Vector2 position, bool save = false)
    {
        var instance = Instantiate(source[id], position, Quaternion.identity);
        instance.name = source[id].name;
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

    public void RemoveInstance(SerializableObject instance)
    {
        ((JArray)level.Root)[instances.IndexOf(instance)].Remove();
        level.Save();

        instances.Remove(instance);
        Destroy(instance.gameObject);
    }

    public void CopyInstance(SerializableObject from, SerializableObject to)
    {
        var properties = new JObject();
        from.Serialize(properties);
        to.Deserialize(properties);

        UpdateInstance(to);
        level.Save();
    }

    public void UpdateInstance(SerializableObject instance)
    {
        ((JArray)level.Root)[instances.IndexOf(instance)].Replace(SerializeInstance(instance));
        level.Save();
    }

    private JToken SerializeInstance(SerializableObject instance)
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