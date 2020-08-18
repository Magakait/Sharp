using UnityEngine;

namespace Sharp.Editor
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public class EditorGrid : MonoBehaviour
    {
        private const int halfSide = 63;

        private Animator animator;

        private void Awake()
        {
            GetComponent<SpriteRenderer>().size = Vector2.one * (halfSide * 2 + 1);
            animator = GetComponent<Animator>();
        }

        public void Toggle(bool visible) =>
            animator.SetBool("Visible", visible);

        public static Vector3 Clamp(Vector3 position)
        {
            position.x = Mathf.Clamp(position.x, -halfSide, halfSide);
            position.y = Mathf.Clamp(position.y, -halfSide, halfSide);
            return position;
        }
    }
}
