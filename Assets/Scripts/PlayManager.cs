using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public JsonFile level;

    private void Awake()
    {
        LevelManager.Main.LoadLevel(level);
        Cursor.visible = false;
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