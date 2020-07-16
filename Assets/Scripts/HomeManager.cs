using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AlKaitagi.SharpCore.Events;

class HomeManager : MonoBehaviour
{
    [SerializeField]
    private Text versionText;

    [Space(10)]
    [SerializeField]
    private LevelManager levelManager;
    [SerializeField]
    private SetManager setManager;

    [Space(10)]
    [SerializeField]
    private GameObject[] singletons;

    [Space(10)]
    [SerializeField]
    private VoidEvent onHome;
    [SerializeField]
    private BoolEvent onCheck;

    private static HomeManager main;

    private void Awake()
    {
        if (main)
        {
            gameObject.SetActive(false);
            foreach (var singleton in singletons)
                Destroy(singleton);
            return;
        }

        main = this;

        foreach (var singleton in singletons)
            DontDestroyOnLoad(singleton);

        levelManager.OnEnable();
        setManager.OnEnable();

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
            onHome.Invoke();
        onCheck.Invoke(home);
    }
}
