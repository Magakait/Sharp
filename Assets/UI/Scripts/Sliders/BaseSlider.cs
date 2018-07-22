using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public abstract class BaseSlider : MonoBehaviour
{
    public FloatEvent onNumber;
    public StringEvent onText;

    public Slider Slider { get; private set; }

    private void Awake() =>
        Slider = GetComponent<Slider>();

    public void Process(float value)
    {
        value = Number(value);
        onNumber.Invoke(value);
        onText.Invoke(Text(value));
    }

    protected abstract float Number(float value);
    protected abstract string Text(float value);
}