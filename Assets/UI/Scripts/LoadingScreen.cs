using System.Collections;

using UnityEngine;

using DG.Tweening;
using UnityEngine.Events;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvas;

    private Tween tween;
    private static LoadingScreen main;

    public static bool Ready => !main.canvas.blocksRaycasts;

    private void Awake()
    {
        if (main)
            return;

        main = this;

        tween = canvas
            .DOFade(1, .15f)
            .SetEase(Ease.InOutQuad)
            .SetUpdate(true);
        tween.Complete();
    }

    private void Start() => MakeTransition(() => { });

    private static void Play(bool isBackwards)
    {
        main.tween.Play(isBackwards);
        main.canvas.blocksRaycasts = !isBackwards;
    }

    private IEnumerator Transition(UnityAction action)
    {
        Play(false);
        yield return Wait();
        action.Invoke();
        yield return Wait();
        Play(true);
    }

    private WaitForSecondsRealtime Wait() => new WaitForSecondsRealtime(tween.Duration());

    public static void MakeTransition(UnityAction action)
    {
        if (Ready)
            main.StartCoroutine(main.Transition(action));
    }
}