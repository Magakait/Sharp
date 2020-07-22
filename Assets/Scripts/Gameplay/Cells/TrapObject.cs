using UnityEngine;
using Newtonsoft.Json.Linq;

public class TrapObject : MonoBehaviour, ISerializable
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state.State == 1)
            collision.GetComponent<UnitComponent>().Kill();
    }

    #region gameplay

    [Header("Gameplay")]
    [SerializeField]
    private CellComponent cell;
    [SerializeField]
    private StateComponent state;

    public void Switch()
    {
        bool active = state.State == 1;
        if (active)
            foreach (UnitComponent unit in cell.GetCollisions<UnitComponent>())
                unit.Kill();

        effect.Emission(active);
    }

    #endregion

    #region animation

    [Header("Animation")]
    [SerializeField]
    private ParticleSystem effect;

    #endregion

    #region serialization

    public void Serialize(JToken token) =>
        token["active"] = state.State == 1;

    public void Deserialize(JToken token) =>
        state.State = (bool)token["active"] ? 1 : 0;

    #endregion
}
