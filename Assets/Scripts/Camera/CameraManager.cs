using UnityEngine;

using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private new Camera camera;
    public static Camera Camera { get; private set; }

    private static CameraManager manager;

    public static Transform Target { get; set; }
    private static Tweener tween;

    private void Awake()
    {
        if (manager)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        Camera = camera;
        manager = this;

        tween = transform
            .DOMove(Position, 2 * Constants.Time)
            .Pause();
    }

    private void Update()
    {
        if (Target)
            Move(Target.position);
    }

    private static Vector3 Offset(Vector3 vector) =>
        new Vector3(vector.x, vector.y, -10);

    public static Vector2 Position
    {
        get
        {
            return new Vector2(manager.transform.position.x, manager.transform.position.y);
        }
        set
        {
            manager.transform.position = Offset(value);
            tween.Pause();
        }
    }

    public static void Move(Vector3 destination) =>
        tween
            .ChangeValues(manager.transform.position, Offset(destination))
            .Restart();
}