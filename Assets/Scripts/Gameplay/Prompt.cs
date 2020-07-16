using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using AlKaitagi.SharpUI;

public class Prompt : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private Button button;

    [Space(10)]
    [SerializeField]
    private CanvasToggle canvasToggle;

    private void Start() =>
        canvasToggle.Visible = true;

    public void Setup(string text, UnityAction action)
    {
        this.text.text = text;
        button.onClick.AddListener(action);
    }
}
