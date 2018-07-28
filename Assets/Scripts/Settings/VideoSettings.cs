using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class VideoSettings : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
        file.Load(Constants.SettingsRoot + "Video.json");
        Load();
    }

    public void Reset() =>
        file.LoadFrom(Constants.SettingsRoot + "Defaults/Video.json");

    #region resolution

    public JsonFile file;
    public Dropdown dropdownResolutions;

    private Resolution[] resolutions;

    private void Load()
    {
        resolutions = Screen.resolutions
            .Skip(7)
            .ToArray();

        dropdownResolutions.AddOptions
        (
            resolutions
                .Select(i => $"{i.width} x {i.height}")
                .ToList()
        );

        dropdownResolutions.RefreshShownValue();
    }

    public void Fullscreen(bool value) =>
        Screen.SetResolution(Screen.width, Screen.height, value);

    public void Size(int index) =>
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);

    #endregion

    #region effects

    public PostProcessProfile postProcessProfile;

    private void ToggleEffect<T>(bool enabled) where T : PostProcessEffectSettings
    {
        T component;
        if (postProcessProfile.TryGetSettings(out component))
            component.enabled.Override(enabled);
    }

    public void Bloom(bool value) => ToggleEffect<Bloom>(value);

    public void ChromaticAberration(bool value) => ToggleEffect<ChromaticAberration>(value);

    public void Vignette(bool value) => ToggleEffect<Vignette>(value);

    public void LensDistortion(bool value) => ToggleEffect<LensDistortion>(value);

    #endregion
}