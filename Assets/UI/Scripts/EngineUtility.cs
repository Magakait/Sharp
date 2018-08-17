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
        foreach (var toggle in group.ActiveToggles())
        {
            toggle.isOn = false;
            toggle.isOn = true;
            break;
        }
    }

    public void Click(Button button) =>
        button.onClick.Invoke();

    public void Toggle(Toggle toggle) =>
        toggle.isOn = !toggle.isOn;

    private static readonly char[] invalidChars = Path.GetInvalidPathChars()
        .Union(Path.GetInvalidFileNameChars())
        .ToArray();
    public void Filter(InputField inputField)
    {
        inputField.text = new string
        (
            inputField.text
                .Where(i => !invalidChars.Contains(i))
                .ToArray()
        );
    }

    #endregion

    #region statics

    public static bool IsOverUI =>
        EventSystem.current.IsPointerOverGameObject();

    public static bool IsInput =>
        EventSystem.current.currentSelectedGameObject;

    public static string NextFile(string directory, string file)
    {
        var extension = Path.GetExtension(file);
        file = Path.GetFileNameWithoutExtension(file);

        var result = string.Empty;
        var Exists = String.IsNullOrEmpty(extension)
            ? (Func<string, bool>)((path) => Directory.Exists(path))
            : (path) => File.Exists(path);

        for (var i = 1; i <= int.MaxValue; i++)
        {
            result = $"{directory}/{file} {i}{extension}";
            if (!Exists(result))
                break;
        }

        return result;
    }

    #endregion

    #region openers

    public void Quit() =>
        Application.Quit();

    public void LoadScene(string scene = null) =>
        SceneManager.LoadScene(String.IsNullOrEmpty(scene) ? SceneManager.GetActiveScene().name : scene);

    public void OpenURL(string url) =>
        Application.OpenURL(url);

    #endregion
}