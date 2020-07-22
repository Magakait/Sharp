using UnityEngine;
using Newtonsoft.Json.Linq;
using DG.Tweening;

public class PortalObject : MonoBehaviour, ISerializable
{
    private void Awake()
    {
        animation = gameObject.AddComponent<TweenArrayComponent>().Init
        (
            DOTween.Sequence().Insert
            (
                frameTransform
                    .DORotate(Constants.Eulers[1], Constants.Time)
                    .SetEase(Ease.Linear)
            )
                .OnComplete(() => { if (state.State == 1) animation[0].Restart(); })
                .Play(),
            DOTween.Sequence().Insert
            (
                frameTransform
                    .DOScale(1.5f, Constants.Time),
                outParticle.transform
                    .DOScale(1.5f, Constants.Time)
            )
                .SetLoops(2, LoopType.Yoyo)
        );

        outParticle.transform.parent = null;
    }

    private void OnDestroy()
    {
        if (outParticle)
            Destroy(outParticle.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state.State == 1)
        {
            MovableComponent movable = collision.GetComponent<MovableComponent>();
            if (movable)
            {
                Teleport(movable);
                if (movable.GetComponent<PlayerObject>())
                    CameraManager.Position = Destination;
            }
        }
    }

    #region gameplay

    [Header("Gameplay")]
    [SerializeField]
    private CellComponent cell;
    [SerializeField]
    private StateComponent state;

    [Space(10)]
    [SerializeField]
    private Vector2 destination;
    public Vector2 Destination
    {
        get => destination;
        set
        {
            destination = value;
            outParticle.transform.position = Destination;
        }
    }

    public void Teleport(MovableComponent movable)
    {
        movable.Position = Destination;
        animation[1].Restart();
    }

    public void Switch()
    {
        bool active = state.State == 1;
        if (active)
        {
            foreach (MovableComponent movable in cell.GetCollisions<MovableComponent>())
                Teleport(movable);

            animation[0].Restart();
        }

        inParticle.Emission(active);
        outParticle.Emission(active);
    }

    #endregion

    #region animation

    [Header("Animation")]
    [SerializeField]
    private Transform frameTransform;
    [SerializeField]
    private ParticleSystem inParticle;
    [SerializeField]
    private ParticleSystem outParticle;

    private new TweenArrayComponent animation;

    #endregion

    #region serialization

    public void Serialize(JToken token)
    {
        token["destination"] = Destination.ToJToken();
        token["active"] = state.State == 1;
    }

    public void Deserialize(JToken token)
    {
        Destination = token["destination"].ToVector();
        state.State = (bool)token["active"] ? 1 : 0;
    }

    #endregion
}
