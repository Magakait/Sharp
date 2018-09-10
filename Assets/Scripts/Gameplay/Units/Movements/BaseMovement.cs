using UnityEngine;

public abstract class BaseMovement : ScriptableObject
{
    public virtual void Idle(MovableComponent movable) { }

    public abstract void Move(MovableComponent movable, int direction);
}