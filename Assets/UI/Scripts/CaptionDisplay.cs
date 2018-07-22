using System.Collections;

using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(CanvasToggle))]
public class CaptionDisplay : MonoBehaviour
{
    public Text caption;

    [Space(10)]
    public float durationTemporary;

    private CanvasToggle canvasToggle;

    private void Awake() => canvasToggle = GetComponent<CanvasToggle>();

    public void Show(string text, bool temporary = true)
    {
        caption.text = text;
        canvasToggle.Visible = true;

        StopAllCoroutines();
        if (temporary)
            StartCoroutine(RoutineHide(durationTemporary + .15f, false));
    }

    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(RoutineHide(0.15f, false));
    }

    private IEnumerator RoutineHide(float delay, bool value)
    {
        yield return new WaitForSeconds(delay);
        canvasToggle.Visible = value;
    }
}