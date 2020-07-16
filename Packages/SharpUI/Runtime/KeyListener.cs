using UnityEngine;
using AlKaitagi.SharpCore.Events;
using AlKaitagi.SharpCore.Variables;

namespace AlKaitagi.SharpUI
{
    public class KeyListener : MonoBehaviour
    {
        [SerializeField]
        private KeyVariable key = null;
        [SerializeField]
        private VoidEvent onDown = null;
        [SerializeField]
        private VoidEvent onUp = null;
        [SerializeField]
        private BoolEvent onHold = null;

        private void Update()
        {
            if (UIUtility.IsInput)
                return;

            if (key.IsDown)
            {
                Down();
                Hold(true);
            }
            else if (key.IsUp)
            {
                Up();
                Hold(false);
            }
        }

        public void Down() =>
            onDown.Invoke();

        public void Up() =>
            onUp.Invoke();

        public void Hold(bool value) =>
            onHold.Invoke(value);
    }
}
