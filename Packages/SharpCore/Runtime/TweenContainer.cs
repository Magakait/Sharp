using UnityEngine;
using DG.Tweening;

namespace Sharp.Core
{
    public class TweenContainer : MonoBehaviour
    {
        private Tween[] tweens;

        public Tween this[int index] => tweens[index];

        public TweenContainer Init(params Tween[] tweens)
        {
            this.tweens = tweens;
            return this;
        }

        private void OnDestroy()
        {
            if (tweens != null)
                foreach (var tween in tweens)
                    tween.Kill();
        }
    }
}
