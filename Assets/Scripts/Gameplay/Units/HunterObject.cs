using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using Newtonsoft.Json.Linq;
using DG.Tweening;

public class HunterObject : SerializableObject
{
    private void Start()
    {
        animation = gameObject.AddComponent<TweenArrayComponent>().Init
        (
            body
                .DOFade(.25f, Constants.Time)
        );

        mark.transform.SetParent(null);
        mark.transform.localScale = Vector3.one;
        ready = true;
    }

    private void OnDestroy()
    {
        if (mark)
            Destroy(mark);
    }

    private void FixedUpdate()
    {
        if (movable.IsMoving)
            return;

        if (!ready)
        {
            ready = true;
            return;
        }

        Cast();
        if (players.Count > 0)
        {
            var destination = Vector2Int.RoundToInt(players[0].transform.position);
            mark.transform.position = new Vector3(destination.x, destination.y);

            ready = false;
            Shift(false);
            rotator.Rotate(new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, destination - movable.Position)));
            movable.Move(destination);
        }
    }

    #region gameplay

    [Header("Gameplay")]
    [SerializeField]
    private new Collider2D collider;
    [SerializeField]
    private MovableComponent movable;
    [SerializeField]
    private RotatorComponent rotator;

    [Space(10)]
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
            effectTransform.localScale = 2 * Distance * Vector3.one;

            foreach (var scaler in particleScalers)
                scaler.Scale();
        }
    }

    private bool ready;

    private static readonly List<PlayerObject> players = new List<PlayerObject>();

    private void Cast() =>
        PhysicsUtility.OverlapBox
        (
            players,
            transform.position,
            effectTransform.localScale,
            Constants.UnitMask
        );

    public void Shift(bool active)
    {
        collider.enabled = active;
        mark.Emission(!active);
        animation[0].Play(active);
    }

    #endregion

    #region animation

    [Header("Animation")]
    [SerializeField]
    private ParticleSystem mark;
    [SerializeField]
    private SpriteRenderer body;

    [Space(10)]
    [SerializeField]
    private Transform effectTransform;
    [SerializeField]
    private ParticleScalerComponent[] particleScalers;

    private new TweenArrayComponent animation;

    #endregion

    #region serialization

    public override void Serialize(JToken token)
    {
        token["distance"] = Distance;
        token["transition"] = movable.Transition;
    }

    public override void Deserialize(JToken token)
    {
        Distance = (int)token["distance"];
        movable.Transition = (float)token["transition"];
    }

    #endregion
}