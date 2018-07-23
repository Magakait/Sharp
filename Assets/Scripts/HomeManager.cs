using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class HomeManager : MonoBehaviour
{
    public VoidEvent onHome;
    public BoolEvent onCheck;

    [Space(10)]
    public Text versionText;
    public JsonFile buffer;

    private static bool loaded;

    private void Awake()
    {
        if (loaded)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }

        loaded = true;
        buffer.Load(Constants.EditorRoot + "Buffer.json");

        versionText.text = Application.version;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() => SceneManager.activeSceneChanged += Check;

    private void Check(Scene oldScene, Scene newScene)
    {
        bool home = newScene.name == "Home";

        if (home)
            onHome.Invoke();
        onCheck.Invoke(home);
    }
}