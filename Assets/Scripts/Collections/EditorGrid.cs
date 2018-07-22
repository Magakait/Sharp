using System.Linq;

using UnityEngine;

using DG.Tweening;

public class EditorGrid : MonoBehaviour
{
    private void Awake()
    {
        SetAnimation();
        spriteRenderer.size = Vector2.one * (halfSide * 2 + 1);
    }

    #region gameplay

    private const int halfSide = 63;
    private readonly static Plane plane = new Plane(Vector3.forward, Vector3.zero);

    public void Toggle(bool value) =>
        animation[0].Play(value);

    public void Find(int id)
    {
        SerializableObject target = LevelManager.Main.instances.FirstOrDefault(i => i.Id == id);
        if (target)
            CameraManager.Move(target.transform.position);
    }

    public static Vector3 Clamp(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, -halfSide, halfSide);
        position.y = Mathf.Clamp(position.y, -halfSide, halfSide);

        return position;
    }

    public static Vector2 MousePosition()
    {
        Ray ray = CameraManager.Camera.ScreenPointToRay(Input.mousePosition);

        float distance;
        plane.Raycast(ray, out distance);

        return ray.GetPoint(distance);
    }

    #endregion

    #region animation

    [Header("Animation")]
    [SerializeField]
    public SpriteRenderer spriteRenderer;

    private new Sequence[] animation;

    private void SetAnimation() =>
        animation = new Sequence[]
        {
            DOTween.Sequence().Insert
            (
                spriteRenderer.material
                    .DOFade(0, .15f)
            )
        };

    #endregion
}