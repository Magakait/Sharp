using UnityEngine;

[CreateAssetMenu(menuName = "Movements/Jump")]
public class JumpMovement : BaseMovement
{
    public override void Move(MovableComponent movable, int direction)
    {
        var position = movable.IntPosition;
        while (MovableComponent.CanMove(position + Constants.Directions[direction]))
            position += Constants.Directions[direction];

        movable.Move(position);
    }
}