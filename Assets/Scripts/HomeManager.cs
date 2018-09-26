using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class HomeManager : MonoBehaviour
{
    [SerializeField]
    private Text versionText;

    [Space(10)]
    [SerializeField]
    private LevelManager levelManager;
    [SerializeField]
    private CollectionManager collectionManager;

    [Space(10)]
    [SerializeField]
    private VoidEvent onHome;
    [SerializeField]
    private BoolEvent onCheck;

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
        DontDestroyOnLoad(gameObject);

        levelManager.OnEnable();
        collectionManager.OnEnable();

        versionText.text = "Version " + Application.version;
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
        {
            CameraManager.Position = Vector2.zero;
            onHome.Invoke();
        }
        onCheck.Invoke(home);
    }
}