using UnityEngine;
using Sharp.Core;
using Sharp.Camera;
using Newtonsoft.Json.Linq;

namespace Sharp.Gameplay
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CellComponent))]
    [RequireComponent(typeof(StateComponent))]
    [RequireComponent(typeof(AudioSource))]
    public class PortalObject : MonoBehaviour, ISerializable
    {
        [Space(10)]
        [SerializeField] private Vector2 destination;
        public Vector2 Destination
        {
            get => destination;
            set
            {
                destination = value;
                outParticle.transform.position = Destination;
            }
        }

        [Space(10)]
        [SerializeField] private ParticleSystem inParticle;
        [SerializeField] private ParticleSystem outParticle;

        private Animator animator;
        private new AudioSource audio;
        private CellComponent cell;
        private StateComponent state;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            audio = GetComponent<AudioSource>();
            cell = GetComponent<CellComponent>();
            state = GetComponent<StateComponent>();

            outParticle.transform.parent = null;
        }

        private void Start() =>
            outParticle.transform.parent = transform;

        private void OnDestroy()
        {
            if (outParticle)
                Destroy(outParticle.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (state.State == 1)
            {
                audio.Play();
                MovableComponent movable = collision.GetComponent<MovableComponent>();
                if (collision.GetComponent<MovableComponent>() is MovableComponent mv)
                {
                    Teleport(mv);
                    if (mv.GetComponent<PlayerObject>())
                        CameraManager.Position = Destination;
                }
            }
        }

        public void Teleport(MovableComponent movable)
        {
            movable.Position = Destination;
            animator.SetTrigger("Teleport");
        }

        public void Switch()
        {
            bool active = state.State == 1;
            if (active)
                foreach (MovableComponent movable in cell.GetCollisions<MovableComponent>())
                    Teleport(movable);

            animator.SetBool("Active", active);
            inParticle.Emission(active);
            outParticle.Emission(active);
        }

        public void Serialize(JToken token)
        {
            token["destination"] = Destination.ToJToken();
            token["active"] = state.State == 1;
        }

        public void Deserialize(JToken token)
        {
            Destination = token["destination"].ToVector();
            state.State = (bool)token["active"] ? 1 : 0;
        }
    }
}
