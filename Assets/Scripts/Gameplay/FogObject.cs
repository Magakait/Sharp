using UnityEngine;
using Sharp.Core;
using Newtonsoft.Json.Linq;

namespace Sharp.Gameplay
{
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(Animator))]
    public class FogObject : MonoBehaviour, ISerializable
    {
        private new CircleCollider2D collider;
        private Animator animator;

        private void Awake()
        {
            collider = GetComponent<CircleCollider2D>();
            animator = GetComponent<Animator>();
        }

        private void Start() =>
            collider.radius = 1f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<PlayerObject>())
                Reveal();
        }

        [SerializeField]
        private bool spread;
        public bool Spread
        {
            get => spread;
            set => spread = value;
        }

        private void Reveal()
        {
            collider.enabled = false;
            if (Spread)
                Invoke("Delay", .075f);

            animator.SetTrigger("Reveal");
            Destroy(gameObject, 1);
        }

        private void Delay()
        {
            if (Physics2D.OverlapPoint(transform.position, Constants.FogMask) is Collider2D cd)
            {
                cd.GetComponent<FogObject>().Reveal();
                Delay();
            }
        }

        public void Serialize(JToken token) =>
            token["spread"] = Spread;

        public void Deserialize(JToken token) =>
            Spread = (bool)token["spread"];
    }
}
