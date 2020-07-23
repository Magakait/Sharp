using Newtonsoft.Json.Linq;

namespace Sharp.Gameplay
{
    public interface ISerializable
    {
        public void Serialize(JToken token);
        public void Deserialize(JToken token);
    }
}
