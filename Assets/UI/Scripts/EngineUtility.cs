using System;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class EngineUtility : ScriptableObject
{
    public static EngineUtility Main { get; private set; }

    private void OnEnable() =>
        Main = this;

    #region UI

    public void Select(ToggleGroup group)
    {
        foreach (Toggle toggle in group.ActiveToggles())
        {
            toggle.isOn = false;
            toggle.isOn = true;
            break;
        }
    }

    public void Click(Button button)
    {
        button.onClick.Invoke();
    }

    public void Toggle(Toggle toggle)
    {
        toggle.isOn = !toggle.isOn;
    }

    private static readonly char[] invalidChars = Path.GetInvalidPathChars()
        .Union(Path.GetInvalidFileNameChars())
        .ToArray();
    public void Filter(InputField inputField)
    {
        inputField.text = new string(inputField.text
            .Where(i => !invalidChars.Contains(i))
            .ToArray());
    }

    #endregion

    #region statics

    public static bool IsOverUI =>
        EventSystem.current.IsPointerOverGameObject();

    public static bool IsInput =>
        EventSystem.current.currentSelectedGameObject;

    public static string NextFile(string directory, string name, string extension)
    {
        string result = string.Empty;
        Func<String, bool> Exists = String.IsNullOrEmpty(extension)
            ? (Func<string, bool>)((path) => Directory.Exists(path))
            : (path) => File.Exists(path);

        for (int i = 1; i <= int.MaxValue; i++)
        {
            result = $"{directory}{name} {i}{extension}";
            if (!Exists(result))
                break;
        }

        return result;
    }

    #endregion

    #region openers

    public void Quit()
    {
        Application.Quit();
    }

    public void OpenScene(string name = null)
    {
        SceneManager.LoadScene(String.IsNullOrEmpty(name) ? SceneManager.GetActiveScene().name : name);
    }

    public void OpenUrl(string url)
    {
        Application.OpenURL(url);
    }

    #endregion
}