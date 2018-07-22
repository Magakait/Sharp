using UnityEngine;

using Newtonsoft.Json.Linq;

public class GateObject : SerializableObject
{
    #region gameplay

    [Header("Gameplay")]
    public CellComponent cell;
    public StateComponent state;

    public bool Open
    {
        get
        {
            return state.State == 1;
        }
        set
        {
            state.State = value ? 1 : 0;
        }
    }

    public void Switch() => 
        cell.Hollowed = !Open;

    #endregion

    #region serialization

    public override void Serialize(JToken token) => 
        token["open"] = Open;

    public override void Deserialize(JToken token) => 
        Open = (bool)token["open"];

    #endregion
}