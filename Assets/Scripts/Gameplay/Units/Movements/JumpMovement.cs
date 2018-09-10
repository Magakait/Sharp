using UnityEngine;

[CreateAssetMenu(menuName = "Movements/Jump")]
public class JumpMovement : BaseMovement
{
    public override void Assign(MovableComponent movable) =>
        movable.Transition /= 2;

    public override void Dispose(MovableComponent movable) =>
        movable.Transition *= 2;

    public override void Move(MovableComponent movable, int direction)
    {
        Vector2 position = movable.IntPosition;
        while (MovableComponent.CanMove(position + Constants.Directions[direction]))
            position += Constants.Directions[direction];

        movable.Move(position);
    }
}