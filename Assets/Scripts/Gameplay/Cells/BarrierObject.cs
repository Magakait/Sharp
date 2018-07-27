using UnityEngine;

using Newtonsoft.Json.Linq;

public class BarrierObject : SerializableObject
{
    private void Start() =>
        Register();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Charges > 0)
        {
            UnitComponent unit = collision.GetComponent<UnitComponent>();
            if (unit)
                unit.Kill();
        }
        else if (Charges == 0 && barrier)
            Unregister();
    }

    #region gameplay

    [Header("Gameplay")]
    public CellComponent cell;

    public Vector2 Target { get; private set; }
    private BarrierObject barrier;

    private int charges;
    public int Charges
    {
        get
        {
            return charges;
        }
        set
        {
            charges = value;

            cell.Hollowed = Charges > 0;
            entropyHalo.Emission(Charges > 0);
            energyHalo.Emission(Charges == 0 && barrier);
        }
    }

    private void Register()
    {
        barrier = PhysicsUtility.Overlap<BarrierObject>(Target, Constants.CellMask);
        if (barrier)
            barrier.Charges++;

        Charges = Charges;
    }

    public void Unregister()
    {
        Charges--;
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

    [Header("Animation")]
    [SerializeField]
    private ParticleSystem entropyHalo;
    [SerializeField]
    private ParticleSystem energyHalo;

    [Space(10)]
    [SerializeField]
    private ParticleSystem pointerEffect;
    [SerializeField]
    private ParticleSystem ambientEffect;

    #endregion

    #region serialization

    public override void Serialize(JToken token) =>
        token["target"] = Target.ToJToken();

    public override void Deserialize(JToken token) =>
        Target = token["target"].ToVector();

    #endregion
}