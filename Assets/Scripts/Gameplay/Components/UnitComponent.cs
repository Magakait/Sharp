using UnityEngine;
using Sharp.Core;
using Sharp.Core.Events;
using DG.Tweening;

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

    public VoidEvent onKill;

    [Space(10)]
    [SerializeField]
    private Transform collapseTransform;
    [SerializeField]
    private ParticleSystem collapseEffect;
    [SerializeField]
    private Behaviour[] toDisable;

    private Tweener tween;

    private void Awake() =>
        tween = collapseTransform
            .DOScale(0, Constants.Time)
            .From()
            .OnRewind(() => Destroy(gameObject))
            .Play();

    private void OnDestroy() =>
        tween.Kill();

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
        tween.PlayBackwards();

        foreach (var behaviour in toDisable)
            behaviour.enabled = false;
    }
}
