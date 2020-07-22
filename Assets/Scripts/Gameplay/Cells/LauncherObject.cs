using UnityEngine;
using Sharp.Core;
using Newtonsoft.Json.Linq;
using DG.Tweening;

public class LauncherObject : MonoBehaviour, ISerializable
{
    private void Awake() =>
        animation = gameObject.AddComponent<TweenArrayComponent>().Init
        (
            DOTween.Sequence().Insert
            (
                leftWing
                    .DOLocalRotate(new Vector3(0, 0, 20), Constants.Time),
                rightWing
                    .DOLocalRotate(new Vector3(0, 0, -20), Constants.Time)
            ),
            DOTween.Sequence().Insert
            (
                pointer
                    .DOLocalMoveY(.5f, Constants.Time)
            )
                .SetLoops(2, LoopType.Yoyo)
        );

    private void Start()
    {
        PhysicsUtility.RaycastDirections(targets, transform.position, Constants.CellMask);
        Check();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targets[state.State])
        {
            MovableComponent movable = collision.GetComponent<MovableComponent>();
            if (movable)
                Launch(movable);
        }
    }

    #region gameplay

    [Header("Gameplay")]
    public CellComponent cell;
    public StateComponent state;

    private readonly Transform[] targets = new Transform[4];

    public void Check()
    {
        bool active = targets[state.State];

        animation[0].Play(active);
        halo.Emission(active);

        if (active)
            foreach (MovableComponent movable in cell.GetCollisions<MovableComponent>())
                Launch(movable);
    }

    public void Launch(MovableComponent movable)
    {
        movable.Transition *= Scale;
        movable.Move(targets[state.State].position);
        movable.Transition /= Scale;

        Instantiate(burst, transform.position, Constants.Rotations[state.State]);
        animation[1].Restart();
    }

    #endregion

    #region animation

    [Header("Animation")]
    [SerializeField]
    private Transform pointer;
    [SerializeField]
    private Transform leftWing;
    [SerializeField]
    private Transform rightWing;

    [Space(10)]
    [SerializeField]
    private ParticleSystem halo;
    [SerializeField]
    private ParticleSystem burst;

    private new TweenArrayComponent animation;

    #endregion

    #region serialization

    [Space(10)]
    [SerializeField]
    private float scale;
    public float Scale
    {
        get => scale;
        private set => scale = value;
    }

    public void Serialize(JToken token)
    {
        token["direction"] = state.State;
        token["scale"] = Scale;
    }

    public void Deserialize(JToken token)
    {
        state.State = (int)token["direction"];
        Scale = (float)token["scale"];
    }

    #endregion
}
