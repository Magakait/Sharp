using UnityEngine;

[CreateAssetMenu(menuName = "Movements/Snake")]
public class SnakeMovement : BaseMovement
{
    public override void Idle(MovableComponent movable) => movable.Move(movable.Direction);

    public override void Move(MovableComponent movable, int direction) => movable.Move(direction);
}