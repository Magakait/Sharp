using UnityEngine;
using Sharp.Core;
using Sharp.Core.Events;

public class UnitComponent : MonoBehaviour
{
    [SerializeField]
    private bool virus;
    public bool Virus => virus;

    [SerializeField]
    private bool killed;
    public bool Killed
    {
        get => killed;
        set => killed = value;
    }
    [SerializeField]
    private ParticleSystem collapseEffect;
    [SerializeField]
    private Behaviour[] toDisable;
    [SerializeField]
    private VoidEvent onKill;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (virus && collision.gameObject.layer == Constants.UnitLayer)
            collision.GetComponent<UnitComponent>().Kill();
    }

    public void Kill()
    {
        if (killed)
            return;

        killed = true;
        onKill.Invoke();

        if (collapseEffect)
            Instantiate(collapseEffect, transform.position, Quaternion.identity);

        foreach (var behaviour in toDisable)
            behaviour.enabled = false;

        Destroy(gameObject);
    }
}
