using UnityEngine;
using UnityEngine.EventSystems;

using Newtonsoft.Json.Linq;
using DG.Tweening;

public class EntranceObject : SerializableObject, IPointerClickHandler
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (enabled && Open)
            CameraManager.Move(transform.position);
    }

    #region animation

    [Header("Animation")]
    [SerializeField]
    private ParticleSystem orbParticle;
    private Transform frameTransform;
    
    private new TweenArrayComponent animation;

    #endregion

    #region serialization

    public bool Open { get; private set; }
    public bool Passed { get; private set; }
    public string Level { get; private set; }
    public Vector2 Next { get; private set; }

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