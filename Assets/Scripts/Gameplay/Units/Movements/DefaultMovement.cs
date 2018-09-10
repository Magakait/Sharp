using UnityEngine;

[CreateAssetMenu(menuName = "Movements/Default")]
public class DefaultMovement : BaseMovement
{
    public override void Move(MovableComponent movable, int direction) => movable.Move(direction);
}