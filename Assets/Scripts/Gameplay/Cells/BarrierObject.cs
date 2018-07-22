using UnityEngine;

public class BarrierObject : SerializableObject
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Orbs > 0)
        {
            UnitComponent unit = collision.GetComponent<UnitComponent>();
            if (unit)
                unit.Kill();
        }
    }

    #region gameplay

    [Header("Gameplay")]
    public CellComponent cell;

    [Space(10)]
    public ParticleSystem effect;

    private int orbs;
    public int Orbs
    {
        get
        {
            return orbs;
        }
        set
        {
            orbs = value;

            cell.Hollowed = Orbs > 0;
            effect.Emission(Orbs > 0);
        }
    }

    #endregion
}