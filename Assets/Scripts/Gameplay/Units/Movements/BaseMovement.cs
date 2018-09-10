using UnityEngine;

[CreateAssetMenu(menuName = "Movements/Base")]
public class BaseMovement : ScriptableObject
{
    public virtual void Assign(MovableComponent movable) { }

    public virtual void Dispose(MovableComponent movable) { }

    public virtual void Idle(MovableComponent movable) { }

    public virtual void Move(MovableComponent movable, int direction) =>
        movable.Move(direction);
}