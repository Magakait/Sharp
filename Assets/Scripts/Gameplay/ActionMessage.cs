using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ActionMessage : MonoBehaviour
{
    public Text bodyText;
    public Text buttonText;
    public Button actionButton;

    [Space(10)]
    public CanvasToggle canvasToggle;

    private void Start() =>
        canvasToggle.Visible = true;

    public void Setup(string body, string button, UnityAction action)
    {
        bodyText.text = body;
        buttonText.text = button;
        actionButton.onClick.AddListener(action);
    }
}