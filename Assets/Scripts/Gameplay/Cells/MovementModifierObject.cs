using UnityEngine;

using DG.Tweening;
using Newtonsoft.Json.Linq;

public class MovementModifierObject : SerializableObject
{
    private void Awake() =>
        animation = gameObject.AddComponent<TweenArrayComponent>().Init
        (
            DOTween.Sequence().Insert
            (
                frameTransform
                    .DOScale(1.25f, 1)
            )
                .SetLoops(-1, LoopType.Yoyo)
                .Play()
        );

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerObject player = collision.GetComponent<PlayerObject>();
        if (player && player.Checkpoint != this)
            player.Movement = movements[Movement];
    }

    [Header("Gameplay")]
    [SerializeField]
    private BaseMovement[] movements;
    [SerializeField]
    private Sprite[] sprites;

    [Space(10)]
    [SerializeField]
    private int movement;
    public int Movement
    {
        get
        {
            return movement;
        }
        set
        {
            movement = value;
            movementRenderer.sprite = sprites[Movement];
        }
    }

    #region animation 

    [Header("Animation")]
    [SerializeField]
    private Transform frameTransform;
    [SerializeField]
    private SpriteRenderer movementRenderer;

    private new TweenArrayComponent animation;

    #endregion

    #region serialization

    public override void Serialize(JToken token) =>
        token["movement"] = Movement;

    public override void Deserialize(JToken token) =>
        Movement = (int)token["movement"];

    #endregion
}