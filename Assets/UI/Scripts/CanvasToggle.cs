using UnityEngine;

using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public class CanvasToggle : MonoBehaviour
{
    [SerializeField]
    private bool visible;
    public bool Visible
    {
        get
        {
            return visible;
        }

        set
        {
            visible = value;

            canvasGroup.blocksRaycasts = Visible;
            animation.Play(Visible);
        }
    }

    [Space(10)]
    [SerializeField]
    private float scale = 1;
    [SerializeField]
    private Vector2 offset;

    private CanvasGroup canvasGroup;
    private new Tween animation;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        animation = DOTween.Sequence().Insert
        (
            transform
                .DOScale(scale * transform.localScale, .15f),
            canvasGroup
                .DOFade(0, .15f),
            GetComponent<RectTransform>()
                .DOAnchorPos(offset, .15f)
                .SetRelative()
        )
            .SetEase(Ease.InOutQuad)
            .SetUpdate(true);

        Visible = Visible;
        if (!Visible)
            animation.Complete();
    }

    private void OnDestroy() =>
        animation.Kill();

    public void Toggle() =>
        Visible = !Visible;
}