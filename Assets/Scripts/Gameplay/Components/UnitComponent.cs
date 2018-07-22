using UnityEngine;

using DG.Tweening;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SerializableObject))]
public class UnitComponent : MonoBehaviour
{
    [Space(10)]
    [SerializeField]
    private bool fragile;
    public bool Fragile => fragile;

    [SerializeField]
    private bool virus;
    public bool Virus => virus;

    private bool killed;

    public VoidEvent onKilled;

    [Space(10)]
    [SerializeField]
    private Transform collapseTransform;
    [SerializeField]
    private ParticleSystem collapseEffect;

    private Tweener tweener;

    private void Awake()
    {
        tweener = collapseTransform
            .DOScale(0, Constants.Time)
            .From()
            .Play();
    }

    private void OnDestroy() =>
        tweener.Kill();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Constants.UnitLayer)
        {
            if (fragile)
                Kill();

            if (virus)
            {
                UnitComponent unit = collision.GetComponent<UnitComponent>();
                if (unit)
                    unit.Kill();
            }
        }
    }

    public void Kill()
    {
        if (killed)
            return;

        killed = true;
        onKilled.Invoke();

        GetComponent<Collider2D>().enabled = false;
        GetComponent<SerializableObject>().enabled = false;

        if (collapseEffect)
            Instantiate(collapseEffect, transform.position, Quaternion.identity);

        tweener.PlayBackwards();
        Destroy(gameObject, Constants.Time);
    }
}
