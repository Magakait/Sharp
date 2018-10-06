using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using Newtonsoft.Json.Linq;
using DG.Tweening;

public class HunterObject : SerializableObject
{
    private void Start()
    {
        projectile.transform.SetParent(null);
        projectile.transform.localScale = Vector3.one;
    }

    private void OnDestroy()
    {
        if (projectile)
            Destroy(projectile.gameObject);
    }

    private void FixedUpdate()
    {
        if (projectile.IsMoving)
            return;

        Cast();
        if (players.Count > 0)
            projectile.Move(Vector2Int.RoundToInt(players[0].transform.position));
    }

    #region gameplay

    [Header("Gameplay")]
    [SerializeField]
    private MovableComponent movable;
    [SerializeField]
    private MovableComponent projectile;

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

    private static readonly List<PlayerObject> players = new List<PlayerObject>();

    private void Cast() =>
        PhysicsUtility.OverlapBox
        (
            players,
            transform.position,
            effectTransform.localScale,
            Constants.UnitMask
        );

    public void Teleport() => movable.Position = projectile.Position;

    #endregion

    #region animation

    [Header("Animation")]
    [SerializeField]
    private Transform effectTransform;
    [SerializeField]
    private ParticleScalerComponent[] particleScalers;

    #endregion

    #region serialization

    public override void Serialize(JToken token)
    {
        token["distance"] = Distance;
        token["transition"] = projectile.Transition;
    }

    public override void Deserialize(JToken token)
    {
        Distance = (int)token["distance"];
        projectile.Transition = (float)token["transition"];
    }

    #endregion
}