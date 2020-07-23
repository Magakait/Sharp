using UnityEngine;
using Sharp.Core.Events;

namespace Sharp.Managers
{
    public class PlayManager : MonoBehaviour
    {
        [SerializeField]
        private BoolEvent onPause;

        private void Awake()
        {
            LevelManager.InstantiateAll();
            SetCursor(false);
        }

        private void SetCursor(bool value)
        {
            Cursor.visible = value;
            Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
        }

        private void SetTime(bool value) => Time.timeScale = value ? 1 : 0;

        public void TogglePause()
        {
            bool pause = Time.timeScale != 0;

            SetTime(!pause);
            SetCursor(pause);

            onPause.Invoke(pause);
        }

        private void OnDestroy()
        {
            SetTime(true);
            SetCursor(true);
        }
    }
}
