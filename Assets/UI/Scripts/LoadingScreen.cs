using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using DG.Tweening;
using UnityEngine.Events;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvas;

    private Tween tween;

    private const float duration = .15f;
    private static LoadingScreen main;

    private void Awake()
    {
        if (main)
        {
            Destroy(gameObject);
            return;
        }

        tween = canvas
            .DOFade(1, duration)
            .SetUpdate(true);

        main = this;
        DontDestroyOnLoad(gameObject);
    }

    private static void Play(bool isBackwards)
    {
        main.tween.Play(isBackwards);
        main.canvas.blocksRaycasts = !isBackwards;
    }

    private IEnumerator Transition(UnityAction action)
    {
        Show();
        yield return new WaitForSecondsRealtime(main.tween.IsPlaying() ? duration : 0);

        action.Invoke();
        Hide();
    }

    public static void Show() => Play(false);

    public static void Hide() => Play(true);

    public static void MakeTransition(UnityAction action) => main.StartCoroutine(main.Transition(action));
}