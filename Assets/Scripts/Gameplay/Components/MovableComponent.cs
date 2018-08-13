using UnityEngine;

using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class MovableComponent : MonoBehaviour
{
    public int Direction { get; private set; }
    
    public Vector2 Position
    {
        get
        {
            return rigidbody.position;
        }
        set
        {
            rigidbody.position = value;
            Stop();
        }
    }
    public Vector2 IntPosition =>
        Vector2Int.RoundToInt(Position);

    public bool IsMoving =>
        tweener.IsPlaying();

    [Space(10)]
    public IntEvent onMove;

    [SerializeField]
    private float transition = .15f;
    public float Transition
    {
        get
        {
            return transition;
        }
        set
        {
            transition = value;
        }
    }

    private new Rigidbody2D rigidbody;
    private Tweener tweener;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        tweener = rigidbody
            .DOMove(Position, 0)
            .SetEase(Ease.Linear)
            .SetUpdate(UpdateType.Fixed);
    }

    private void OnDestroy() =>
        tweener.Kill();

    public void Stop() =>
        tweener.Pause();

    public bool CanMove(int direction)
    {
        CellComponent cell = PhysicsUtility
            .Overlap<CellComponent>(IntPosition + Constants.Directions[direction], Constants.CellMask);
        return cell && !cell.Hollowed;
    }

    public void Move(int direction) =>
        Move(IntPosition + Constants.Directions[direction]);

    public void Move(Vector2 destination)
    {
        tweener
            .Pause()
            .ChangeValues(Position, destination, Transition * Vector2.Distance(Position, destination))
            .Restart();

        Direction = Index(destination - Position);
        onMove.Invoke(Direction);
    }

    public static int Index(Vector2 vector)
    {
        if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
            return vector.x > 0 ? 1 : 3;
        else
            return vector.y > 0 ? 0 : 2;
    }
}