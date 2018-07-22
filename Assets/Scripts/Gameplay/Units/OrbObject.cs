using UnityEngine;

using Newtonsoft.Json.Linq;

public class OrbObject : SerializableObject
{
    private void Start() =>
        Register();

    #region gameplay

    public Vector2 Target { get; private set; }

    private BarrierObject wall;

    private void Register()
    {
        wall = PhysicsUtility.Overlap<BarrierObject>(Target, Constants.CellMask);
        if (wall)
            wall.Orbs++;
    }

    public void Unregister()
    {
        if (!wall)
            return;

        Instantiate
        (
            pointer,
            transform.position,
            Quaternion.FromToRotation(Vector3.up, Target - (Vector2)transform.position)
        );

        if (--wall.Orbs == 0)
            Instantiate(ambient, transform.position, Quaternion.identity);
    }

    #endregion

    #region animation

    [Header("Animation")]
    [SerializeField]
    private ParticleSystem pointer;
    [SerializeField]
    private ParticleSystem ambient;

    #endregion

    #region serialization

    public override void Serialize(JToken token) =>
        token["target"] = Target.ToJToken();

    public override void Deserialize(JToken token) =>
        Target = token["target"].ToVector();

    #endregion
}
