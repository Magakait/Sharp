using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Sharp.Core;

namespace Sharp.Settings
{
    public class VideoSettings : MonoBehaviour
    {
        private void Awake()
        {
            file.Load(Constants.SettingsRoot + "Video.json");
            Load();
        }

        public void Reset() =>
            file.LoadFrom(Constants.SettingsRoot + "Defaults\\Video.json");

        #region resolution

        [SerializeField]
        private JsonFile file;
        [SerializeField]
        private Dropdown dropdownResolutions;

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
            Screen.SetResolution
            (
                resolutions[index].width,
                resolutions[index].height,
                Screen.fullScreen
            );

        #endregion

        #region render

        public void VSync(bool value) =>
            QualitySettings.vSyncCount = value ? 1 : 0;

        public void TargetFps(int value) =>
            Application.targetFrameRate = 30 * (value + 2);

        #endregion

        #region effects

        [SerializeField]
        private VolumeProfile postProcessProfile;

        private void ToggleEffect<T>(bool enabled) where T : VolumeComponent
        {
            T component;
            if (postProcessProfile.TryGet(out component))
                component.active = enabled;
        }

        public void Bloom(bool value) =>
            ToggleEffect<Bloom>(value);

        public void ChromaticAberration(bool value) =>
            ToggleEffect<ChromaticAberration>(value);

        public void Vignette(bool value) =>
            ToggleEffect<Vignette>(value);

        public void LensDistortion(bool value) =>
            ToggleEffect<LensDistortion>(value);

        public void Grain(bool value) =>
            ToggleEffect<FilmGrain>(value);

        #endregion
    }
}
