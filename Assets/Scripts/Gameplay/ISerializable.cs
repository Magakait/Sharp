using Newtonsoft.Json.Linq;

public interface ISerializable
{
    public void Serialize(JToken token);
    public void Deserialize(JToken token);
}
