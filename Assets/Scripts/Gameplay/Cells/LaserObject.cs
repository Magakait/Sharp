using System.Collections.Generic;
using UnityEngine;
using Sharp.Core;
using Newtonsoft.Json.Linq;

namespace Sharp.Gameplay
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(StateComponent))]
    [RequireComponent(typeof(AudioSource))]
    public class LaserObject : MonoBehaviour, ISerializable
    {
        [SerializeField]
        private ParticleScalerComponent distanceScaler;

        private Animator animator;
        private StateComponent state;
        private new AudioSource audio;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            state = GetComponent<StateComponent>();
            audio = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            active = true;
            enabled = true;

            var main = distanceScaler.ParticleSystem.main;
            main.startColor = main.startColor.color.Fade(1);

            distanceScaler.ParticleSystem.Refresh();
            animator.SetTrigger("Burst");
            audio.Play();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (Persistent)
                active = true;
        }

        private void FixedUpdate()
        {
            if (active)
            {
                Burst();
                active = false;
            }
            else
            {
                var main = distanceScaler.ParticleSystem.main;
                main.startColor = main.startColor.color.Fade(0.3f);
                enabled = false;
            }
        }

        private readonly static List<UnitComponent> units = new List<UnitComponent>();

        private bool active;

        private void Burst()
        {
            var from = (Vector2)transform.position + .5f * Constants.Directions[Direction];
            var to = from + Distance * Constants.Directions[Direction];

            PhysicsUtility.OverlapArea(units, from, to, Constants.UnitMask);
            foreach (var unit in units)
                unit.Kill();
        }

        public int Direction
        {
            get => state.State;
            private set => state.State = value;
        }

        [SerializeField]
        private int distance;
        public int Distance
        {
            get => distance;
            private set
            {
                distance = value;
                distanceScaler.Scale(new Vector3(Distance, 0, 0));
            }
        }
        [SerializeField]
        private bool persistent;
        public bool Persistent
        {
            get => persistent;
            private set
            {
                persistent = value;
                animator.SetBool("Persistent", Persistent);
            }
        }

        public void Serialize(JToken token)
        {
            token["direction"] = Direction;
            token["distance"] = Distance;
            token["persistent"] = Persistent;
        }

        public void Deserialize(JToken token)
        {
            Direction = (int)token["direction"];
            Distance = (int)token["distance"];
            Persistent = (bool)token["persistent"];
        }
    }
}
