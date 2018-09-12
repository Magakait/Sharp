using UnityEngine;

public class PlayManager : MonoBehaviour
{
    private void Awake()
    {
        LevelManager.Main.LoadLevel();
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