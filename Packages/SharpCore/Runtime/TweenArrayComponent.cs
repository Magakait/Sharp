using UnityEngine;
using DG.Tweening;

namespace Sharp.Core
{
    public class TweenArrayComponent : MonoBehaviour
    {
        private Tween[] tweens;

        public Tween this[int index] => tweens[index];

        public TweenArrayComponent Init(params Tween[] tweens)
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
