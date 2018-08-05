using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using DG.Tweening;
using Newtonsoft.Json.Linq;

public class LaserObject : SerializableObject
{
    private void Awake() =>
        animation = gameObject.AddComponent<TweenArrayComponent>().Init
        (
            DOTween.Sequence().Insert
            (
                leftTransform
                    .DOLocalMoveX(-.2f, Constants.Time),
                rightTransform
                    .DOLocalMoveX(.2f, Constants.Time)
            )
                .SetLoops(2, LoopType.Yoyo),
            DOTween.Sequence().Insert
            (
                persistencyTransform
                    .DOScale(0, Constants.Time)
            )
        );

    private void OnTriggerEnter2D(Collider2D other)
    {
        Active = true;
        distanceEffect.Refresh();

        Burst();

        animation[0].Restart();
        if (!persistent)
            Active = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Active)
            Burst();
    }

    private void OnTriggerExit2D(Collider2D other) =>
        Active = false;

    #region gameplay

    [Space(10)]
    [SerializeField]
    private StateComponent state;

    private readonly static List<UnitComponent> units = new List<UnitComponent>();

    private bool active;
    public bool Active
    {
        get
        {
            return active;
        }
        set
        {
            active = value;

            var main = distanceEffect.main;
            main.startColor = main.startColor.color.Fade(Active ? 1 : .2f);
        }
    }

    private void Burst()
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
    private Transform leftTransform;
    [SerializeField]
    private Transform rightTransform;
    [SerializeField]
    private Transform persistencyTransform;

    [Space(10)]
    [SerializeField]
    private ParticleSystem distanceEffect;
    [SerializeField]
    private ParticleScalerComponent distanceScaler;

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

    [Header("Serialization")]
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
            distanceScaler.Scale(new Vector3(Distance, 0, 0));
            distanceScaler.transform.localPosition = .5f * new Vector3(0, Distance, 0);
        }
    }

    [SerializeField]
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
            animation[1].Play(Persistent);
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