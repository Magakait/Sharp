using UnityEngine;

using DG.Tweening;
using Newtonsoft.Json.Linq;

public class MovementObject : SerializableObject
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerObject player = collision.GetComponent<PlayerObject>();
        if (player && player.Checkpoint != this)
            player.Movement = movements[Movement];
    }

    [Space(10)]
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
            renderer.sprite = sprites[Movement];
        }
    }

    [Space(10)]
    [SerializeField]
    private new SpriteRenderer renderer;

    public override void Serialize(JToken token) =>
        token["movement"] = Movement;

    public override void Deserialize(JToken token) =>
        Movement = (int)token["movement"];
}