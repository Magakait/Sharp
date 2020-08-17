using System.Collections.Generic;
using UnityEngine;
using Sharp.Core;
using Newtonsoft.Json.Linq;

namespace Sharp.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(MovableComponent))]
    [RequireComponent(typeof(RotatorComponent))]
    public class HunterObject : MonoBehaviour, ISerializable
    {
        [SerializeField] private int distance;
        public int Distance
        {
            get => distance;
            set
            {
                distance = value;
                area.Scale(2 * Distance * Vector3.one);
            }
        }
        [SerializeField] private bool local;
        public bool Local
        {
            get => local;
            set
            {
                local = value;
                animator.SetBool("Local", Local);
            }
        }

        [Space(10)]
        [SerializeField] private ParticleSystem mark;
        [SerializeField] private ParticleScalerComponent area;

        private new Collider2D collider;
        private Animator animator;
        private MovableComponent movable;
        private RotatorComponent rotator;

        private bool ready;

        private void Awake()
        {
            collider = GetComponent<Collider2D>();
            animator = GetComponent<Animator>();
            movable = GetComponent<MovableComponent>();
            rotator = GetComponent<RotatorComponent>();
        }

        private void Start()
        {
            area.transform.parent = null;
            mark.transform.parent = null;
            mark.transform.localScale = Vector3.one;
            ready = true;
        }

        private void OnDestroy()
        {
            if (mark)
                Destroy(mark);
        }

        private void FixedUpdate()
        {
            if (!ready)
            {
                ready = true;
                return;
            }

            Cast();
            if (players.Count > 0)
            {
                var destination = Vector2Int.RoundToInt(players[0].transform.position);
                mark.transform.position = new Vector3(destination.x, destination.y);

                ready = false;

                movable.Move(destination);
                rotator.Rotate(new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, destination - movable.Position)));
            }
        }

        private static readonly List<PlayerObject> players = new List<PlayerObject>();

        private void Cast() =>
            PhysicsUtility.OverlapBox
            (
                players,
                area.transform.position,
                area.transform.localScale,
                Constants.UnitMask
            );

        public void Shift(bool shift)
        {
            collider.enabled = shift;
            enabled = shift;
            mark.Emission(!shift);
            if (shift && !Local)
                area.transform.position = transform.position;
            animator.SetBool("Shift", !shift);
        }

        public void Serialize(JToken token)
        {
            token["transition"] = movable.Transition;
            token["distance"] = Distance;
            token["local"] = Local;
        }

        public void Deserialize(JToken token)
        {
            movable.Transition = (float)token["transition"];
            Distance = (int)token["distance"];
            Local = (bool)token["local"];
        }
    }
}
