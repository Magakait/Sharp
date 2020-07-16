using UnityEngine;
using AlKaitagi.SharpCore.Events;

namespace AlKaitagi.SharpUI
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField, Range(1, 5)]
        private int updateRate = 4;
        [SerializeField]
        private FloatEvent onUpdate = null;

        private int frames;
        private float deltaTime;

        private void Update()
        {
            frames++;
            deltaTime += Time.unscaledDeltaTime;
            if (deltaTime > 1f / updateRate)
            {
                onUpdate.Invoke(frames / deltaTime);
                frames = 0;
                deltaTime -= 1f / updateRate;
            }
        }
    }
}
