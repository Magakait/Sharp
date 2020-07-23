using UnityEngine;

namespace Sharp.Gameplay
{
    [CreateAssetMenu(menuName = "Movements/Base")]
    public class BaseMovement : ScriptableObject
    {
        [SerializeField]
        private Sprite icon;
        public Sprite Icon => icon;

        public virtual void Idle(MovableComponent movable) { }

        public virtual void Move(MovableComponent movable, int direction) => movable.Move(direction);
    }
}
