using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using DG.Tweening;

[RequireComponent(typeof(Collider2D))]
public class CellComponent : MonoBehaviour
{
    [SerializeField]
    private bool hollowed;
    public bool Hollowed
    {
        get
        {
            return hollowed;
        }
        set
        {
            hollowed = value;
            animation.Play(!Hollowed);
        }
    }

    [Space(10)]
    [SerializeField]
    private SpriteRenderer coreSprite;
    [SerializeField]
    private SpriteRenderer frameSprite;

    private new Tween animation;

    private void Awake() =>
        animation = DOTween.Sequence().Insert
        (
            frameSprite.material
                .DOFade(0, Constants.Time),
            frameSprite.transform
                .DOScale(3, Constants.Time),
            coreSprite.transform
                .DOScale(1, Constants.Time)
        );

    private void OnDestroy() =>
        animation.Kill();

    private void OnTriggerEnter2D(Collider2D collision) =>
        collisions.Add(collision);

    private void OnTriggerExit2D(Collider2D collision) =>
        collisions.Remove(collision);

    private readonly List<Collider2D> collisions = new List<Collider2D>();

    public IEnumerable<T> GetCollisions<T>() where T : Component => 
        collisions
            .Where(i => i)
            .Select(i => i.GetComponent<T>())
            .Where(i => i);
}