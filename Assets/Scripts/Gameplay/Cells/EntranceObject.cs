using UnityEngine;

using Newtonsoft.Json.Linq;
using DG.Tweening;

public class EntranceObject : SerializableObject
{
    private void Awake() =>
        animation = gameObject.AddComponent<TweenArrayComponent>().Init
        (
            DOTween.Sequence().Insert
            (
                frameTransform
                    .DOScale(0, Constants.Time)
            )
        );

    private void OnMouseDown()
    {
        if (enabled && Open)
            CameraManager.Move(transform.position);
    }

    #region animation

    [Header("Animation")]
    [SerializeField]
    private Transform frameTransform;
    [SerializeField]
    private ParticleSystem orbParticle;
    [SerializeField]
    private LineRenderer lineNext;

    private new TweenArrayComponent animation;

    #endregion

    #region serialization

    public bool Open { get; private set; } = true;
    public bool Passed { get; private set; }
    public string Level { get; private set; }

    private Vector2 next;
    public Vector2 Next
    {
        get
        {
            return next;
        }
        set
        {
            next = value;

            lineNext.SetPosition(0, transform.position);
            lineNext.SetPosition(1, (Next + (Vector2)transform.position) / 2);
            lineNext.SetPosition(2, Next);
        }
    }

    public override void Serialize(JToken token)
    {
        token["open"] = Open;
        token["level"] = Level;
        token["next"] = Next.ToJToken();
    }

    public override void Deserialize(JToken token)
    {
        Open = (bool)token["open"];
        Level = (string)token["level"];
        Next = token["next"].ToVector();
    }

    #endregion
}