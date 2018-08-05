using UnityEngine;

using Newtonsoft.Json.Linq;

public class BarrierObject : SerializableObject
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Charges > 0)
            collision.GetComponent<UnitComponent>().Kill();
    }

    #region gameplay

    [Header("Gameplay")]
    public CellComponent cell;

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
            haloEffect.Emission(Charges > 0);
        }
    }

    #endregion

    #region animation

    [Header("Animation")]
    [SerializeField]
    private ParticleSystem haloEffect;

    #endregion
}