using UnityEngine;
using UnityEngine.UI;

using Newtonsoft.Json.Linq;

public class EditorProperties : MonoBehaviour
{
    public Text header;
    public CanvasToggle canvasToggle;

    [Space(10)]
    public JsonFile catalog;
    public JsonFile buffer;

    [Space(10)]
    public Transform parentPanel;
    public BaseWidget[] widgets;

    private SerializableObject selected;

    private void Awake()
    {
        catalog.Load(Constants.EditorRoot + "Properties.json");
        BaseWidget.editorProperties = this;
    }

    public void Load(SerializableObject selected)
    {
        parentPanel.Clear();
        if (!selected)
        {
            canvasToggle.Visible = false;
            return;
        }

        this.selected = selected;
        header.text = selected.name;

        var properties = (JArray)catalog[selected.Id.ToString()];
        if (properties != null)
        {
            buffer.Root = new JObject();
            selected.Serialize(buffer.Root);

            foreach (var property in properties)
                Instantiate(widgets[(int)property["type"]], parentPanel)
                    .Load(property);
        }

        canvasToggle.Visible = parentPanel.childCount > 0;
    }

    public void Save()
    {
        selected.Deserialize(buffer.Root);
        LevelManager.UpdateInstance(selected);
    }
}