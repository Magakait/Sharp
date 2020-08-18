using UnityEngine;
using Sharp.Core;

namespace Sharp.Settings
{
    public class AudioSettings : MonoBehaviour
    {
        public JsonFile file;

        private void Awake() =>
            file.Load(Constants.SettingsRoot + "Audio.json");

        public void Reset() =>
            file.LoadFrom(Constants.SettingsRoot + "Defaults\\Audio.json");

        public void SetMute(bool mute) =>
            AudioListener.pause = mute;

        public void SetVolume(float volume) =>
            AudioListener.volume = .1f * volume;
    }
}
