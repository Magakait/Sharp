using System.Collections.Generic;

using UnityEngine;

using DG.Tweening;
using Newtonsoft.Json.Linq;

public class LaserObject : SerializableObject
{
    private void Start() =>
        animation = gameObject.AddComponent<TweenArrayComponent>().Init
        (
            DOTween.Sequence().Insert
            (
                leftWing
                    .DOLocalMoveX(-.2f, Constants.Time),
                rightWing
                    .DOLocalMoveX(.2f, Constants.Time)
            )
                .SetLoops(2, LoopType.Yoyo)
        );

    private void OnTriggerEnter2D(Collider2D other)
    {
        Shoot();
        if (Persistent)
            shooting = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (shooting)
            Shoot();
    }

    private void OnTriggerExit2D(Collider2D other) =>
        shooting = false;

    #region gameplay

    [Space(10)]
    [SerializeField]
    private StateComponent state;

    private readonly static List<UnitComponent> units = new List<UnitComponent>();
    private bool shooting;

    private void Shoot()
    {
        var from = (Vector2)transform.position + .5f * Constants.Directions[Direction];
        var to = from + Distance * Constants.Directions[Direction];

        PhysicsUtility.OverlapArea(units, from, to, Constants.UnitMask);
        foreach (var unit in units)
            unit.Kill();
    }

    #endregion

    #region animation

    [Header("Animation")]
    [SerializeField]
    private Transform leftWing;
    [SerializeField]
    private Transform rightWing;

    [Space(10)]
    [SerializeField]
    private ParticleScalerComponent distanceEffect;
    [SerializeField]
    private ParticleSystem persistencyEffect;

    private new TweenArrayComponent animation;

    #endregion

    #region serialization

    public int Direction
    {
        get
        {
            return state.State;
        }
        set
        {
            state.State = value;
        }
    }

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
            distanceEffect.Scale(new Vector3(0, Distance, 0));
        }
    }

    private bool persistent;
    public bool Persistent
    {
        get
        {
            return persistent;
        }
        set
        {
            persistent = value;
            persistencyEffect.Emission(Persistent);
        }
    }

    public override void Serialize(JToken token)
    {
        token["direction"] = Direction;
        token["distance"] = Distance;
        token["persistent"] = Persistent;
    }

    public override void Deserialize(JToken token)
    {
        Direction = (int)token["direction"];
        Distance = (int)token["distance"];
        Persistent = (bool)token["persistent"];
    }

    #endregion

}