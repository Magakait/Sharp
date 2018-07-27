using System.IO;

using UnityEngine;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour
{
    public Button editorButton;

    [Space(10)]
    public JsonFile level;

    private void Awake()
    {
        LevelManager.Main.LoadLevel(level);
        Cursor.visible = false;
        editorButton.interactable = new DirectoryInfo(level.Directory).Parent.Name == "Local";
    }

    public void TogglePause()
    {
        Cursor.visible = !Cursor.visible;
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
        Time.timeScale = 1;
    }
}