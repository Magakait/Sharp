using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public JsonFile level;

    private void Awake()
    {
        LevelManager.LoadLevel(level);
        ToggleCursor(false);
    }

    public void TogglePause()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        ToggleCursor(Time.timeScale == 0);
    }

    private void ToggleCursor(bool value)
    {
        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void OnDestroy() =>
        Time.timeScale = 1;
}