using UnityEngine;

namespace AlKaitagi.SharpUI
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(RectTransform))]
    public class CanvasToggle : MonoBehaviour
    {
        [SerializeField]
        private bool visible;
        public bool Visible
        {
            get => visible;
            set
            {
                visible = value;
                canvasGroup.blocksRaycasts = Visible;
                enabled = true;
                onToggle.Invoke(Visible);
            }
        }
        [SerializeField]
        private float scale = 1;
        [SerializeField]
        private Vector3 offset = Vector3.zero;
        [SerializeField]
        private float duration = .1f;
        [SerializeField]
        private BoolEvent onToggle = null;

        private float timer = 0;

        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;

        private Vector3 startPosition;
        private Vector3 endPosition;

        private Vector3 startScale;
        private Vector3 endScale;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();

            startPosition = rectTransform.anchoredPosition;
            endPosition = startPosition + offset;

            startScale = transform.localScale;
            endScale = scale * startScale;

            if (!Visible)
            {
                canvasGroup.blocksRaycasts = false;
                timer = 1;
                Assign();
            }
        }

        private void OnValidate() =>
            GetComponent<CanvasGroup>().alpha = Visible ? 1 : 0;

        private void Update()
        {
            var timer = Mathf.Clamp01(this.timer + (Visible ? -1 : 1) * Time.unscaledDeltaTime / duration);
            if (timer != this.timer)
            {
                this.timer = timer;
                Assign();
            }
            else
                enabled = false;
        }

        private void Assign()
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, timer);
            transform.localScale = Vector3.Lerp(startScale, endScale, timer);
            rectTransform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, timer);
        }

        public void Toggle() =>
            Visible = !Visible;
    }
}
