using UnityEngine;

using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    private static CameraFollow main;

    public static Transform Target { get; set; }
    private static Tweener tween;

    private void Awake()
    {
        if (main)
            return;

        main = this;
        tween = transform.DOMove(Position, 2 * Constants.Time);
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
            return new Vector2(main.transform.position.x, main.transform.position.y);
        }
        set
        {
            main.transform.position = Offset(value);
            tween.Pause();
        }
    }

    public static void Move(Vector3 destination) => 
        tween
            .ChangeValues(main.transform.position, Offset(destination))
            .Restart();
}