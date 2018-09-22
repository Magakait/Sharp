using UnityEngine;

using DG.Tweening;
using Newtonsoft.Json.Linq;

public class MovementObject : SerializableObject
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerObject player = collision.GetComponent<PlayerObject>();
        if (player && player.Checkpoint != this)
        {
            player.Movement = movements[Movement];
            player.GetComponent<MovableComponent>().Transition = Transition;
        }
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
        private set
        {
            movement = value;
            renderer.sprite = sprites[Movement];
        }
    }

    [SerializeField]
    private float transition;
    public float Transition
    {
        get
        {
            return transition;
        }
        private set
        {
            transition = value;
        }
    }

    [Space(10)]
    [SerializeField]
    private new SpriteRenderer renderer;

    public override void Serialize(JToken token)
    {
        token["movement"] = Movement;
        token["transition"] = Transition;
    }

    public override void Deserialize(JToken token)
    {
        Movement = (int)token["movement"];
        Transition = (float)token["transition"];
    }
}