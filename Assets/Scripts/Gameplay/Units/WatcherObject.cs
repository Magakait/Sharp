using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using Newtonsoft.Json.Linq;
using DG.Tweening;

public class WatcherObject : SerializableObject
{
    private void Start() =>
        animation = gameObject.AddComponent<TweenArrayComponent>().Init
        (
            DOTween.Sequence().Insert
            (
                delayTransform
                    .DOScale(1, delay),
                spikesTransform
                    .DOScale(1, Constants.Time)
            )
                .OnComplete(() => Explode())
        );

    private void FixedUpdate()
    {
        Cast();
        animation[0].Play(!units.FirstOrDefault(unit => unit.GetComponent<PlayerObject>()));
    }

    #region gameplay

    [Header("Gameplay")]
    [SerializeField]
    private int distance;
    public int Distance
    {
        get
        {
            return distance;
        }

        set
        {
            distance = value;
            maskTransform.localScale = (1 + Distance * 2) * Vector3.one;
        }
    }

    [SerializeField]
    private float delay;

    private readonly List<UnitComponent> units = new List<UnitComponent>();

    private void Cast()
    {
        PhysicsUtility.CastBox
        (
            units,
            transform.position,
            maskTransform.localScale,
            Constants.UnitMask
        );
    }

    public void Explode()
    {
        foreach (UnitComponent unit in units.Where(unit => !unit.Virus))
            unit.Kill();

        Instantiate(burstParticle, maskTransform);
    }

    #endregion

    #region animation

    [Header("Animation")]
    [SerializeField]
    private Transform spikesTransform;
    [SerializeField]
    private Transform delayTransform;
    [SerializeField]
    private Transform maskTransform;

    [Space(10)]
    [SerializeField]
    private ParticleSystem burstParticle;

    private new TweenArrayComponent animation;

    #endregion

    #region serialization

    public override void Serialize(JToken token)
    {
        token["distance"] = Distance;
        token["delay"] = delay;
    }

    public override void Deserialize(JToken token)
    {
        Distance = (int)token["distance"];
        delay = (float)token["delay"];
    }

    #endregion
}