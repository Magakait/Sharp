using UnityEngine;

using DG.Tweening;
using UnityEngine.InputSystem;

public class EditorGrid : MonoBehaviour
{
    private void Awake()
    {
        animation = gameObject.AddComponent<TweenArrayComponent>().Init
        (
            DOTween.Sequence().Insert(spriteRenderer.DOFade(0, .2f))
        );

        spriteRenderer.size = Vector2.one * (halfSide * 2 + 1);
    }

    #region gameplay

    private const int halfSide = 63;
    private readonly static Plane plane = new Plane(Vector3.forward, Vector3.zero);

    public void Toggle(bool value) =>
        animation[0].Play(value);

    public static Vector3 Clamp(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, -halfSide, halfSide);
        position.y = Mathf.Clamp(position.y, -halfSide, halfSide);

        return position;
    }

    public static Vector2 MousePosition()
    {
        Ray ray = CameraManager.Camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        float distance;
        plane.Raycast(ray, out distance);

        return ray.GetPoint(distance);
    }

    #endregion

    #region animation

    [Header("Animation")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private new TweenArrayComponent animation;

    #endregion
}
