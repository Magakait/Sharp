using UnityEngine;

using Newtonsoft.Json.Linq;

public class ActionObject : SerializableObject
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerObject player = collision.GetComponent<PlayerObject>();
        if (player)
        {
            player.Action = actions[Action];
            player.Cooldown = Cooldown;
        }
    }

    [Space(10)]
    [SerializeField]
    private BaseAction[] actions;
    [SerializeField]
    private SpriteRenderer[] shapes;

    [Space(10)]
    [SerializeField]
    private int action;
    public int Action
    {
        get
        {
            return action;
        }
        private set
        {
            action = value;
            foreach (var shape in shapes)
                shape.sprite = actions[Action].Shape;
        }
    }

    [SerializeField]
    private float cooldown;
    public float Cooldown
    {
        get
        {
            return cooldown;
        }
        private set
        {
            cooldown = value;
        }
    }

    public override void Serialize(JToken token)
    {
        token["action"] = Action;
        token["cooldown"] = Cooldown;
    }

    public override void Deserialize(JToken token)
    {
        Action = (int)token["action"];
        Cooldown = (float)token["cooldown"];
    }
}