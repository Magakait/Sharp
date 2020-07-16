using UnityEngine;

namespace AlKaitagi.SharpUI
{
    [ExecuteInEditMode]
    public class RadialUI : MonoBehaviour
    {
        [SerializeField]
        private float radius = 100;
        [SerializeField]
        private float offset = 0;

        public void Update()
        {
            var arc = 360f / transform.childCount;
            for (int i = 0; i < transform.childCount; i++)
            {
                var angle = Mathf.Deg2Rad * (offset + arc * i);
                transform.GetChild(i).localPosition = radius * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            }
        }
    }
}
