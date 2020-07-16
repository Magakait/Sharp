using UnityEngine;
using AlKaitagi.SharpCore.Variables;

namespace AlKaitagi.SharpCore.Painters
{
    public abstract class BasePainter : MonoBehaviour
    {
        [SerializeField]
        private ColorVariable variable;
        public ColorVariable Variable
        {
            get => variable;
            set
            {
                variable = value;
                Refresh();
            }
        }

        private void Awake() =>
            Refresh();

        public abstract void Refresh();

        protected static Color Fade(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
    }
}
