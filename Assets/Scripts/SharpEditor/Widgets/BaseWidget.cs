using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public abstract class BaseWidget : MonoBehaviour
{
    [SerializeField]
    private Text title;

    public static EditorProperties editorProperties;

    protected bool ready;
    private JToken buffer;

    public void Load(JToken property, JToken buffer)
    {
        ready = false;

        title.text = (string)property["name"];
        this.buffer = buffer;
        Read(buffer[title.text], property["attributes"]);

        ready = true;
    }

    public void Save()
    {
        if (ready)
        {
            buffer[title.text] = Write();
            editorProperties.Save();
        }
    }

    protected abstract void Read(JToken value, JToken attributes);
    protected abstract JToken Write();
}
