using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    [SerializeField]
    private Color color;
    public Color Color
    {
        get
        {
            return color;
        }

        set
        {
            color = ApplyNoise(value, noise);
            OnValidate();
        }
    }

    public Image image;

    [Space(10), Range(0, 1)]
    public float noise;

    [Space(10)]
    public ColorEvent onPick;

    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        image.color = Color;
    }

    public void Pick()
    {
        onPick.Invoke(Color);
    }

    private static Color ApplyNoise(Color color, float noise)
    {
        if (noise != 0)
        {
            color.r = Mathf.PingPong(color.r + Random.Range(-noise, noise), 1);
            color.g = Mathf.PingPong(color.g + Random.Range(-noise, noise), 1);
            color.b = Mathf.PingPong(color.b + Random.Range(-noise, noise), 1);
        }

        return color;
    }
}