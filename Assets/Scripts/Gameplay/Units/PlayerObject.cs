using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sharp.UI;
using Sharp.Core;
using Sharp.Core.Variables;
using Sharp.Camera;
using Newtonsoft.Json.Linq;

namespace Sharp.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(MovableComponent))]
    public class PlayerObject : MonoBehaviour, ISerializable
    {
        private void Awake()
        {
            collider = GetComponent<Collider2D>();
            Movable = GetComponent<MovableComponent>();
            CameraManager.Position = transform.position;
        }

        private bool started = false;

        private void Start()
        {
            started = true;
            CameraFollow.Target = transform;
        }

        private void Update()
        {
            if (Time.timeScale == 0)
                return;

            Rotate();
            Buffer();
            if (!Movable.IsMoving)
                Move();
            if (cooldownEffect.emission.enabled && actionKey.IsDown)
                StartCoroutine(Act());
        }

        public MovableComponent Movable { get; private set; }
        private new Collider2D collider;

        [SerializeField]
        private KeyVariable sprintKey;
        [SerializeField]
        private KeyVariable[] directionKeys;
        [SerializeField]
        private KeyVariable[] rotationKeys;
        [SerializeField]
        private KeyVariable actionKey;

        [Space(10)]
        [SerializeField] private Prompt prompt;
        [SerializeField] private BaseMovement movement;
        public BaseMovement Movement
        {
            get => movement;
            set
            {
                movement = value;
                icon.sprite = Movement.Icon;
                if (started)
                    Instantiate(assignEffect, transform.position, Quaternion.identity);
            }
        }
        [SerializeField] private BaseAction action;
        public BaseAction Action
        {
            get => action;
            set
            {
                action = value;
                shape.sprite = Action.Shape;
                cooldownEffect.Emission(Action.name != "Base");
                if (started)
                    Instantiate(assignEffect, transform.position, Quaternion.identity);
            }
        }
        [SerializeField] private float cooldown;
        public float Cooldown
        {
            get => cooldown;
            set => cooldown = value;
        }

        private CheckpointObject checkpoint;
        public CheckpointObject Checkpoint
        {
            get => checkpoint;
            set
            {
                checkpoint = value;
                if (started)
                    Instantiate(assignEffect, transform.position, Quaternion.identity);
            }
        }

        [Space(10)]
        [SerializeField]
        private SpriteRenderer icon;
        [SerializeField]
        private SpriteRenderer shape;
        [SerializeField]
        private ParticleSystem assignEffect;
        [SerializeField]
        private ParticleSystem cooldownEffect;

        private readonly List<int> moves = new List<int>();

        private void Buffer()
        {
            var sprint = Keyboard.current[sprintKey].isPressed;
            var stopSprint = Keyboard.current[sprintKey].wasReleasedThisFrame;

            for (var i = 0; i < 4; i++)
                if (directionKeys[i].IsDown)
                {
                    moves.Remove(i);
                    moves.Add(i);
                    break;
                }
                else if (sprint && Keyboard.current[directionKeys[i]].isPressed)
                {
                    moves.Remove(i);
                    moves.Add(i);
                }
                else if (stopSprint || (sprint && Keyboard.current[directionKeys[i]].wasReleasedThisFrame))
                    moves.Remove(i);
        }

        private void Rotate()
        {
            if (rotationKeys[0].IsDown)
                Movable.Direction--;
            else if (rotationKeys[1].IsDown)
                Movable.Direction++;
        }

        private void Move()
        {
            bool moved = false;

            foreach (var i in moves)
                if (Movable.CanMove(i))
                {
                    movement.Move(Movable, i);
                    moved = true;
                    break;
                }

            if (!moved)
                movement.Idle(Movable);

            moves.Clear();
        }

        private IEnumerator Act()
        {
            action.Do(this);
            cooldownEffect.Emission(false);
            if (action.Effect)
                Instantiate
                (
                    action.Effect,
                    transform.position,
                    Constants.Rotations[Movable.Direction]
                );

            yield return new WaitForSeconds(Cooldown);
            cooldownEffect.Emission(true);
        }

        public void CheckSpawn()
        {
            if (ExitObject.Passed)
                Instantiate(this.prompt, Movable.Position, Quaternion.identity)
                    .Setup("Home", () => UIUtility.Main.LoadScene("Home"));
            else if (Checkpoint)
                Checkpoint.StartCoroutine(Checkpoint.Spawn());
            else
                Instantiate(this.prompt, Movable.Position, Quaternion.identity)
                    .Setup("Restart", () => UIUtility.Main.ReloadScene());
        }

        public void Serialize(JToken token) { }
        public void Deserialize(JToken token) { }
    }
}
