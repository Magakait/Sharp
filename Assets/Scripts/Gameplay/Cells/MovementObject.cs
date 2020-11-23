using UnityEngine;
using Newtonsoft.Json.Linq;

namespace Sharp.Gameplay
{
    [RequireComponent(typeof(AudioSource))]
    public class MovementObject : MonoBehaviour, ISerializable
    {
        private new AudioSource audio;

        private void Awake() =>
            audio = GetComponent<AudioSource>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<PlayerObject>() is PlayerObject p)
            {
                if (p.Movement != movements[Movement])
                    audio.Play();
                p.Movement = movements[Movement];
                p.Movable.Transition = Transition;
            }
        }

        [Space(10)]
        [SerializeField]
        private BaseMovement[] movements;
        [SerializeField]
        private SpriteRenderer[] icons;

        [Space(10)]
        [SerializeField]
        private int movement;
        public int Movement
        {
            get => movement;
            private set
            {
                movement = value;
                foreach (var icon in icons)
                    icon.sprite = movements[Movement].Icon;
            }
        }

        [SerializeField]
        private float transition;
        public float Transition
        {
            get => transition;
            private set => transition = value;
        }

        public void Serialize(JToken token)
        {
            token["movement"] = Movement;
            token["transition"] = Transition;
        }

        public void Deserialize(JToken token)
        {
            Movement = (int)token["movement"];
            Transition = (float)token["transition"];
        }
    }
}
