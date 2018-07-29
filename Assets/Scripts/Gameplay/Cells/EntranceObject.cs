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
            ),
            DOTween.Sequence().Insert
            (
                lineNext.transform
                    .DOScale(1, Constants.Time)
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
    private LineRenderer lineNext;

    private new TweenArrayComponent animation;

    #endregion

    #region serialization

    private bool open;
    public bool Open
    {
        get 
        {
            return open;
        }
        private set
        {
            open = value;
            animation[0].Play(Open);
        }
    }
    
    private bool passed;
    public bool Passed
    {
        get
        {
            return passed;
        }
        set
        {
            passed = value;
            animation[1].Play(!Passed);

            var entrance = PhysicsUtility.Overlap<EntranceObject>(Next, Constants.CellMask);
            if (entrance && !entrance.Open)
            {
                entrance.Open = true;
                CameraManager.Move(Next);
            }
        }
    }
    public string Level { get; private set; }

    private Vector2 next;
    public Vector2 Next
    {
        get
        {
            return next;
        }
        private set
        {
            next = value;

            var position = (Vector2)transform.position;
            var offset = .5f * (Next - position).normalized;

            lineNext.SetPosition(0, position + offset);
            lineNext.SetPosition(1, (position - Next) / 2);
            lineNext.SetPosition(2, Next - offset);
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