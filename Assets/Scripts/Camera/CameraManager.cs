using UnityEngine;

using DG.Tweening;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour
{
    public static Camera Camera { get; private set; }

    public static Vector2 Position
    {
        get
        {
            return new Vector2(Camera.transform.localPosition.x, Camera.transform.localPosition.y);
        }
        set
        {
            Camera.transform.localPosition = new Vector3(value.x, value.y, -10);
        }
    }

    public static Vector3 Rotation
    {
        get
        {
            return Camera.transform.localEulerAngles;
        }
        set
        {
            Camera.transform.localEulerAngles = value;
        }
    }

    public static float FieldOfView
    {
        get
        {
            return Camera.fieldOfView;
        }
        set
        {
            Camera.fieldOfView = value;
        }
    }

    private static Tweener positionTweener;
    private static Tweener zoomTweener;

    private void Awake()
    {
        if (Camera)
            return;
        Camera = GetComponent<Camera>();

        positionTweener = DOTween.To
        (
            () => Position,
            v => Position = v,
            Position,
            Constants.Time
        );
        zoomTweener = DOTween.To
        (
            () => FieldOfView,
            f => FieldOfView = f,
            FieldOfView,
            Constants.Time
        );
    }

    public static void Move(Vector2 position, float scale = 2) =>
        positionTweener
            .ChangeValues(Position, position, scale * Constants.Time)
            .Restart();

    public static void Zoom(float fieldOfView, float scale = 1) =>
        zoomTweener
            .ChangeValues(FieldOfView, fieldOfView, scale * Constants.Time)
            .Restart();
}