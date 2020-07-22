using UnityEngine;
using Newtonsoft.Json.Linq;

public class OrbObject : MonoBehaviour, ISerializable
{
    private void Start() =>
        Register();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerObject>())
        {
            Unregister();

            unit.Killed = false;
            unit.Kill();
        }
    }

    #region gameplay

    [Space(10)]
    [SerializeField]
    private UnitComponent unit;

    private BarrierObject barrier;

    private void Register()
    {
        barrier = PhysicsUtility.Overlap<BarrierObject>(Target, Constants.CellMask);
        if (barrier)
            barrier.Charges++;
    }

    private void Unregister()
    {
        if (!barrier)
            return;

        if (--barrier.Charges == 0)
            Instantiate(ambientEffect, transform.position, Quaternion.identity);

        Instantiate
        (
            pointerEffect,
            transform.position,
            Quaternion.FromToRotation(Vector3.up, Target - (Vector2)transform.position)
        );
    }

    #endregion

    #region animation

    [Space(10)]
    [SerializeField]
    private ParticleSystem pointerEffect;
    [SerializeField]
    private ParticleSystem ambientEffect;

    #endregion

    #region serialization

    public Vector2 Target { get; private set; }

    public void Serialize(JToken token) =>
        token["target"] = Target.ToJToken();

    public void Deserialize(JToken token) =>
        Target = token["target"].ToVector();

    #endregion
}
