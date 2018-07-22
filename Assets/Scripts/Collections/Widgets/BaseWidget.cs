using UnityEngine;
using UnityEngine.UI;

using Newtonsoft.Json.Linq;

public abstract class BaseWidget : MonoBehaviour
{
    [SerializeField]
    private Text title;
    [SerializeField]
    private JsonFile buffer;

    public static EditorProperties editorProperties;

    protected bool ready;

    public void Load(JToken property)
    {
        ready = false;

        title.text = (string)property["name"];
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