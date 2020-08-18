using Newtonsoft.Json.Linq;

namespace Sharp.Gameplay
{
    public interface ISerializable
    {
        void Serialize(JToken token);
        void Deserialize(JToken token);
    }
}
