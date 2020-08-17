using UnityEngine;
using Sharp.Core;
using Sharp.Core.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class MovableComponent : MonoBehaviour
{
    private int direction;
    public int Direction
    {
        get => direction;
        set
        {
            direction = (int)Mathf.Repeat(value, 4);
            onDirectionChange.Invoke(Direction);
        }
    }

    public Vector2 Position
    {
        get => rigidbody.position;
        set
        {
            rigidbody.position = value;
            Stop();
        }
    }

    public Vector2 IntPosition => Vector2Int.RoundToInt(Position);
    public bool IsMoving => target.HasValue;

    [Space(10)]
    [SerializeField]
    private IntEvent onDirectionChange;
    [SerializeField]
    private VoidEvent onMoveStart;
    [SerializeField]
    private VoidEvent onMoveStop;

    [SerializeField]
    private float transition = .15f;
    public float Transition
    {
        get => transition;
        set => transition = value;
    }

    private new Rigidbody2D rigidbody;
    private Vector2? target = null;

    private void Awake() =>
        rigidbody = GetComponent<Rigidbody2D>();

    private void FixedUpdate()
    {
        if (!target.HasValue)
            return;

        var step = 1 / transition * Time.fixedDeltaTime;
        rigidbody.MovePosition(Vector2.MoveTowards(Position, target.Value, step));
        if ((Position - target.Value).sqrMagnitude <= .01f)
            Stop();
    }

    public bool CanMove(int direction) =>
        CanMove(IntPosition + Constants.Directions[direction]);

    public static bool CanMove(Vector2 position)
    {
        CellComponent cell = PhysicsUtility
            .Overlap<CellComponent>(position, Constants.CellMask);
        return cell && !cell.Hollowed;
    }

    public void Move(int direction) =>
        Move(IntPosition + Constants.Directions[direction]);

    public void Move(Vector2 point)
    {
        target = point;
        Direction = DirectionTo(point - Position);
        onMoveStart.Invoke();
    }

    public void Stop()
    {
        target = null;
        onMoveStop.Invoke();
    }

    public static int DirectionTo(Vector2 destination) =>
        Mathf.Abs(destination.x) > Mathf.Abs(destination.y)
            ? destination.x > 0 ? 1 : 3
            : destination.y > 0 ? 0 : 2;
}
