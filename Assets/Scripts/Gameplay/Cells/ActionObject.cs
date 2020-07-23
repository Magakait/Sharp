using UnityEngine;
using Newtonsoft.Json.Linq;

namespace Sharp.Gameplay
{
    public class ActionObject : MonoBehaviour, ISerializable
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
        public float Cooldown { get => cooldown; private set => cooldown = value; }

        public void Serialize(JToken token)
        {
            token["action"] = Action;
            token["cooldown"] = Cooldown;
        }

        public void Deserialize(JToken token)
        {
            Action = (int)token["action"];
            Cooldown = (float)token["cooldown"];
        }
    }
}
