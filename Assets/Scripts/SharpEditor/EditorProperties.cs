using UnityEngine;
using UnityEngine.UI;

using Newtonsoft.Json.Linq;

public class EditorProperties : MonoBehaviour
{
    [SerializeField]
    private JsonFile catalog;

    [Space(10)]
    [SerializeField]
    private Text header;
    [SerializeField]
    private CanvasToggle canvasToggle;

    [Space(10)]
    [SerializeField]
    private Transform parentPanel;
    [SerializeField]
    private BaseWidget[] widgets;

    private SerializableObject selected;
    private JToken buffer = new JObject();

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
            selected.Serialize(buffer);

            foreach (var property in properties)
                Instantiate(widgets[(int)property["type"]], parentPanel)
                    .Load(property, buffer);
        }

        canvasToggle.Visible = parentPanel.childCount > 0;
    }

    public void Save()
    {
        selected.Deserialize(buffer);
        LevelManager.Main.UpdateInstance(selected);
    }
}