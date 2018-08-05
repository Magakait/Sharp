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
                    .DOLocalMoveX(-.1f, Constants.Time),
                rightTransform
                    .DOLocalMoveX(.1f, Constants.Time)
            )
        );

    private void OnTriggerEnter2D(Collider2D other)
    {
        active = true;

        var main = distanceEffect.main;
        main.startColor = main.startColor.color.Fade(1);

        distanceEffect.Refresh();
        animation[0].PlayForward();
    }

    private void OnTriggerStay2D(Collider2D other) =>
        active = true;

    private void FixedUpdate()
    {
        if (active)
        {
            Burst();
            active = false;
        }
        else
        {
            var main = distanceEffect.main;
            main.startColor = main.startColor.color.Fade(.2f);

            animation[0].PlayBackwards();
        }
    }

    #region gameplay

    [Space(10)]
    [SerializeField]
    private StateComponent state;

    private readonly static List<UnitComponent> units = new List<UnitComponent>();

    private bool active;

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
        private set
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
        private set
        {
            distance = value;
            distanceScaler.Scale(new Vector3(Distance, 0, 0));
            distanceScaler.transform.localPosition = .5f * new Vector3(0, Distance, 0);
        }
    }

    public override void Serialize(JToken token)
    {
        token["direction"] = Direction;
        token["distance"] = Distance;
    }

    public override void Deserialize(JToken token)
    {
        Direction = (int)token["direction"];
        Distance = (int)token["distance"];
    }

    #endregion

}