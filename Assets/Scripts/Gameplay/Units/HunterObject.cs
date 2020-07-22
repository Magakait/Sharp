using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using DG.Tweening;

public class HunterObject : MonoBehaviour, ISerializable
{
    private void Start()
    {
        animation = gameObject.AddComponent<TweenArrayComponent>().Init
        (
            body
                .DOFade(.4f, Constants.Time)
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

            movable.Move(destination);
            rotator.Rotate(new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, destination - movable.Position)));
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
        get => distance;
        set
        {
            distance = value;
            area.Scale(2 * Distance * Vector3.one);
        }
    }

    private bool ready;

    private static readonly List<PlayerObject> players = new List<PlayerObject>();

    private void Cast() =>
        PhysicsUtility.OverlapBox
        (
            players,
            transform.position,
            area.transform.localScale,
            Constants.UnitMask
        );

    public void Shift(bool active)
    {
        collider.enabled = active;
        enabled = active;
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
    [SerializeField]
    private ParticleScalerComponent area;

    private new TweenArrayComponent animation;

    #endregion

    #region serialization

    public void Serialize(JToken token)
    {
        token["transition"] = movable.Transition;
        token["distance"] = Distance;
    }

    public void Deserialize(JToken token)
    {
        movable.Transition = (float)token["transition"];
        Distance = (int)token["distance"];
    }

    #endregion
}
