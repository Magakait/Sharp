using UnityEngine;
using Sharp.Core;
using Newtonsoft.Json.Linq;

namespace Sharp.Gameplay
{
    public class LauncherObject : MonoBehaviour, ISerializable
    {
        private Animator animator;

        private void Awake() =>
            animator = GetComponent<Animator>();

        private void Start()
        {
            PhysicsUtility.RaycastDirections(targets, transform.position, Constants.CellMask);
            Check();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (targets[state.State])
            {
                MovableComponent movable = collision.GetComponent<MovableComponent>();
                if (movable)
                    Launch(movable);
            }
        }

        #region gameplay

        [Header("Gameplay")]
        public CellComponent cell;
        public StateComponent state;

        private readonly Transform[] targets = new Transform[4];

        public void Check()
        {
            bool active = targets[state.State];

            animator.SetBool("Active", active);
            halo.Emission(active);

            if (active)
                foreach (MovableComponent movable in cell.GetCollisions<MovableComponent>())
                    Launch(movable);
        }

        public void Launch(MovableComponent movable)
        {
            movable.Transition *= Scale;
            movable.Move(targets[state.State].position);
            movable.Transition /= Scale;

            Instantiate(burst, transform.position, Constants.Rotations[state.State]);
            animator.SetTrigger("Launch");
        }

        #endregion

        [Space(10)]
        [SerializeField]
        private ParticleSystem halo;
        [SerializeField]
        private ParticleSystem burst;

        #region serialization

        [Space(10)]
        [SerializeField]
        private float scale;
        public float Scale
        {
            get => scale;
            private set => scale = value;
        }

        public void Serialize(JToken token)
        {
            token["direction"] = state.State;
            token["scale"] = Scale;
        }

        public void Deserialize(JToken token)
        {
            state.State = (int)token["direction"];
            Scale = (float)token["scale"];
        }

        #endregion
    }
}
