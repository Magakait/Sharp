using UnityEngine;
using Sharp.Camera;

namespace Sharp.Editor
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public class EditorGrid : MonoBehaviour
    {
        private const int Side = 126;

        private Animator animator;

        private void Awake()
        {
            GetComponent<SpriteRenderer>().size = Vector2.one * (Side + 1);
            animator = GetComponent<Animator>();
        }

        private void Start() =>
            CameraPan.Side = Side;

        public void Toggle(bool visible) =>
            animator.SetBool("Visible", visible);
    }
}
