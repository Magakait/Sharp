using System.Collections.Generic;

using UnityEngine;

using DG.Tweening;
using Newtonsoft.Json.Linq;

public class SwitcherObject : SerializableObject
{
    private void Awake()
    {
        animation = gameObject.AddComponent<TweenArrayComponent>().Init
        (
            DOTween.Sequence().Insert
            (
                innerTransform
                    .DOScale(0, Constants.Time)
            ),
            DOTween.Sequence().Insert
            (
                outerTransform
                    .DOScale(0, Constants.Time)
            ),
            DOTween.Sequence().Insert
            (
                bodyTransform
                    .DORotate(Constants.Eulers[1], Constants.Time)
            )
        );

        effectTransform.parent = null;
    }

    private void Start() =>
        PhysicsUtility.CastBox
        (
            targets,
            effectTransform.position,
            effectTransform.localScale,
            Constants.CellMask
        );

    private void OnDestroy()
    {
        if (effectTransform)
            Destroy(effectTransform.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Enter)
        {
            Switch(true);
            animation[2].Restart();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (Exit)
        {
            Switch(false);

            animation[2].Complete();
            animation[2].SmoothRewind();
        }
    }

    #region gameplay

    [Header("Gameplay")]
    [SerializeField]
    private bool enter;
    public bool Enter
    {
        get
        {
            return enter;
        }
        set
        {
            enter = value;
            animation[0].Play(Enter);
        }
    }

    [SerializeField]
    private bool exit;
    public bool Exit
    {
        get
        {
            return exit;
        }
        set
        {
            exit = value;
            animation[1].Play(Exit);
        }
    }

    [SerializeField]
    private Vector2 origin;
    [SerializeField]
    private Vector2 offset;

    private readonly List<StateComponent> targets = new List<StateComponent>();

    private void Place(Vector2 origin, Vector2 offset)
    {
        this.origin = origin;
        this.offset = offset;

        effectTransform.position = origin + .5f * offset;
        effectTransform.localScale = Vector2.one + new Vector2(Mathf.Abs(offset.x), Mathf.Abs(offset.y));

        foreach (ParticleScalerComponent scaler in particleScalers)
            scaler.Scale();
    }

    private void Switch(bool isUp)
    {
        int delta = isUp ? 1 : -1;
        targets.ForEach(i => i.State += delta);
    }

    #endregion

    #region animation

    [Header("Animation")]
    [SerializeField]
    private Transform bodyTransform;
    [SerializeField]
    private Transform innerTransform;
    [SerializeField]
    private Transform outerTransform;

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
        token["enter"] = Enter;
        token["exit"] = Exit;

        token["origin"] = origin.ToJToken();
        token["offset"] = offset.ToJToken();
    }

    public override void Deserialize(JToken token)
    {
        Enter = (bool)token["enter"];
        Exit = (bool)token["exit"];

        Place(token["origin"].ToVector(), token["offset"].ToVector());
    }

    #endregion
}