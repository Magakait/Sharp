using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class HomeManager : MonoBehaviour
{
    public VoidEvent onHome;
    public BoolEvent onCheck;

    [Space(10)]
    public Text versionText;
    public LevelManager levelManager;
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
        levelManager.OnEnable();
        buffer.Load(Constants.EditorRoot + "Buffer.json");

        versionText.text = Application.version;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += Check;
        onHome.Invoke();
        onCheck.Invoke(true);
    }

    private void Check(Scene current, Scene next)
    {
        bool home = next.name == "Home";

        if (home)
            onHome.Invoke();
        onCheck.Invoke(home);
    }
}