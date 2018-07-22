using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class SkinSettings : MonoBehaviour
{
    public Toggle baseToggle;
    public ToggleGroup toggleGroup;

    [Space(10)]
    public JsonFile file;
    public SpriteVariable variable;
    public StringLoader loader;

    private void Awake() =>
        file.Load(Constants.SettingsRoot + "Cosmetics.json");

    public void List(string name)
    {
        toggleGroup.transform.Clear();

        foreach (string item in Directory
            .GetFiles(Constants.SkinRoot, "*.png")
            .Select(i => Path.GetFileNameWithoutExtension(i)))
        {
            Toggle toggle = Instantiate(baseToggle, toggleGroup.transform);
            toggle.group = toggleGroup;

            toggle.transform.GetChild(1).GetComponent<Text>().text = item;
            toggle.onValueChanged.AddListener(value => Load(item));

            if (item == name)
                toggle.isOn = true;
        }
    }

    private void Load(string name)
    {
        loader.Save(name);

        Texture2D texture = new Texture2D(256, 256, TextureFormat.ARGB32, false);
        texture.LoadImage(File.ReadAllBytes(Constants.SkinRoot + name + ".png"));

        variable.Value = Sprite.Create
            (
                texture,
                new Rect(0, 0, texture.width, texture.height),
                Vector2.one * .5f,
                texture.width
            );

        BaseCosmetic<SpriteVariable>.RefreshAll();
    }
}