using UnityEngine;

namespace Sharp.Gameplay
{
    [CreateAssetMenu(menuName = "Movements/Snake")]
    public class SnakeMovement : BaseMovement
    {
        public override void Idle(MovableComponent movable)
        {
            foreach (var i in new int[] { 0, 2, 1, 3 })
            {
                var direction = (movable.Direction + i) % 4;
                if (movable.CanMove(direction))
                {
                    movable.Move(direction);
                    return;
                }
            }
        }
    }
}
