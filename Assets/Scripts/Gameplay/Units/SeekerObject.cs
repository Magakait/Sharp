using UnityEngine;
using Newtonsoft.Json.Linq;

namespace Sharp.Gameplay
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(MovableComponent))]
    public class SeekerObject : MonoBehaviour, ISerializable
    {
        [SerializeField]
        private string sequence;

        private int index;
        private MovableComponent movable;

        private void Awake() =>
            movable = GetComponent<MovableComponent>();

        private void Start() =>
            GetComponent<Animator>().SetFloat("Speed", 1 / movable.Transition);

        private void Update()
        {
            if (string.IsNullOrEmpty(sequence))
                return;

            Check();
            Move();
        }

        private static readonly bool[] checks = new bool[4];
        private void Check()
        {
            for (var i = 0; i < checks.Length; i++)
                checks[i] = movable.CanMove(i);
        }

        private void Move()
        {
            var index = this.index;
            do
            {
                var direction = sequence[index] - '0';
                if (checks[direction])
                {
                    movable.Move(direction);
                    this.index = index;
                    break;
                }
                else
                    index = (index + 1) % sequence.Length;
            } while (index != this.index);
        }

        public void Serialize(JToken token)
        {
            token["transition"] = movable.Transition;
            token["sequence"] = sequence;
        }

        public void Deserialize(JToken token)
        {
            movable.Transition = (float)token["transition"];
            sequence = (string)token["sequence"];
        }
    }
}
