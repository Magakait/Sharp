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

    [Space(10)]
    public JsonFile buffer;
    public JsonFile misc;

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
        misc.Load(Constants.SettingsRoot + "Misc.json");

        versionText.text = Application.version;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += Check;
        Check(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void Check(Scene scene, LoadSceneMode mode)
    {
        bool home = scene.name == "Home";

        if (home)
            onHome.Invoke();
        onCheck.Invoke(home);
    }
}