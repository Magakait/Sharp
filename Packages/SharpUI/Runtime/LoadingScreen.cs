using System;
using System.Collections;
using UnityEngine;

namespace AlKaitagi.SharpUI
{
    [RequireComponent(typeof(CanvasToggle))]
    public class LoadingScreen : MonoBehaviour
    {
        private static LoadingScreen main;
        private CanvasToggle canvas;

        private void Awake()
        {
            if (main)
                return;

            main = this;
            canvas = GetComponent<CanvasToggle>();
        }

        private void Start() =>
            MakeTransition(() => { });

        private IEnumerator Transition(Action action)
        {
            WaitForSecondsRealtime Wait() =>
               new WaitForSecondsRealtime(canvas.Duration);

            canvas.Visible = true;
            yield return Wait();
            action.Invoke();
            yield return Wait();
            canvas.Visible = false;
        }

        public static void MakeTransition(Action action)
        {
            if (!main.canvas.Visible)
                main.StartCoroutine(main.Transition(action));
        }
    }
}
