using UnityEngine;

using Newtonsoft.Json.Linq;

public class SerializableObject : MonoBehaviour
{
    [SerializeField]
    private int id;
    public int Id => id;

    public virtual void Serialize(JToken token) { }

    public virtual void Deserialize(JToken token) { }
}