using UnityEngine;
using Newtonsoft.Json.Linq;

namespace Sharp.Gameplay
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CellComponent))]
    [RequireComponent(typeof(StateComponent))]
    public class GateObject : MonoBehaviour, ISerializable
    {
        private Animator animator;
        private CellComponent cell;
        private StateComponent state;

        private void Awake()
        {
            cell = GetComponent<CellComponent>();
            state = GetComponent<StateComponent>();
            animator = GetComponent<Animator>();
        }

        public void Switch()
        {
            cell.Hollowed = !Open;
            animator.SetBool("Open", Open);
        }

        public bool Open
        {
            get => state.State == 1;
            private set => state.State = value ? 1 : 0;
        }

        public void Serialize(JToken token) =>
            token["open"] = Open;

        public void Deserialize(JToken token) =>
            Open = (bool)token["open"];
    }
}
