using UnityEngine;
using UnityEngine.UI;

using Newtonsoft.Json.Linq;

public class VectorWidget : BaseWidget
{
    [Space(10)]
    [SerializeField]
    private InputField inputX;
    [SerializeField]
    private InputField inputY;

    public void Validate(InputField input)
    {
        ready = false;

        int x;
        int.TryParse(input.text, out x);
        input.text = x.ToString();

        ready = true;
    }

    protected override void Read(JToken value, JToken attribute)
    {
        Vector2 vector = value.ToVector();

        inputX.text = vector.x.ToString();
        inputY.text = vector.y.ToString();
    }

    protected override JToken Write()
    {
        int x;
        int y;

        int.TryParse(inputX.text, out x);
        int.TryParse(inputY.text, out y);

        return new Vector2(x, y).ToJToken();
    }
}