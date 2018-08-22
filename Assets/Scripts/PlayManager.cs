using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public JsonFile level;

    private void Awake()
    {
        LevelManager.LoadLevel(level);
        SetCursor(false);
    }

    public void TogglePause()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        SetCursor(Time.timeScale == 0);
    }

    private void SetCursor(bool value)
    {
        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
        SetCursor(true);
    }
}