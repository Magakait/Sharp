using UnityEngine;

public class ColorCode : MonoBehaviour
{
    public ColorEvent onParse;
    public StringEvent onDisplay;

    public void Parse(string value)
    {
        Color color;
        ColorUtility.TryParseHtmlString($"#{value}", out color);

        onParse.Invoke(color);
    }

    public void Display(Color color)
    {
        onDisplay.Invoke(ColorUtility.ToHtmlStringRGB(color));
    }

    public void Display(ColorVariable variable)
    {
        Display(variable.Value);
    }
}