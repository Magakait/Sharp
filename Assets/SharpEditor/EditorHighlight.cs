using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Sharp.UI;
using Sharp.Core.Variables;
using Sharp.Managers;
using Sharp.Camera;

namespace Sharp.Editor
{
    [RequireComponent(typeof(Animator))]
    public class EditorHighlight : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer frameSprite;
        [SerializeField]
        private SpriteRenderer selectionSprite;
        [Space(10)]
        [SerializeField]
        private Text positionText;
        [SerializeField]
        private Text layerText;
        [SerializeField]
        private Text objectText;
        [Space(10)]
        [SerializeField]
        private KeyVariable copyKey;
        [SerializeField]
        private EditorProperties propertiesManager;

        private bool dragging;
        private bool Dragging
        {
            get => dragging;
            set
            {
                dragging = value;
                animator.SetBool("Dragging", Dragging);
            }
        }
        private int layer;
        public int Layer
        {
            get => layer;
            set
            {
                layer = value;
                ClearInput();
            }
        }
        public string SourceName { get; set; }

        private GameObject selected;
        private GameObject Selected
        {
            get => selected;
            set
            {
                selected = value;

                propertiesManager.Load(Selected);
                animator.SetBool("Selected", Selected);
            }
        }

        private bool copied;
        private GameObject target;
        private Animator animator;

        private void Awake() =>
            animator = GetComponent<Animator>();

        private void Update()
        {
            TargetGrid();
            DisplayInfo();

            if (!UIUtility.IsOverUI)
                ProcessMouse();
        }

        private void TargetGrid()
        {
            Vector3 position = Vector3Int.RoundToInt(EditorGrid.Clamp(CameraManager.WorldMouse));
            Collider2D collider = Physics2D.OverlapPoint(position, 1 << Layer);

            if (Dragging)
            {
                if (!collider)
                {
                    if (Keyboard.current[copyKey].isPressed && !copied && target.name != "Player" && target.name != "Exit")
                    {
                        copied = true;

                        GameObject copy = LevelManager.AddInstance(target.name, target.transform.position, true);
                        LevelManager.CopyProperties(target, copy);
                    }

                    target.transform.position = position;
                }
            }
            else
                target = collider?.gameObject;

            frameSprite.transform.position = target ? target.transform.position : position;
            if (Selected)
                selectionSprite.transform.position = Selected.transform.position;
        }

        private void DisplayInfo()
        {
            positionText.text = $"{frameSprite.transform.position.x}, {frameSprite.transform.position.y}";
            layerText.text = LayerMask.LayerToName(Layer);
            objectText.text = target ? $"<b>{target.name}</b>" : SourceName;
        }

        private void ProcessMouse()
        {
            if (target)
            {
                if (!Dragging && Mouse.current.leftButton.wasPressedThisFrame)
                {
                    Dragging = true;
                    Selected = Selected == target ? null : target;
                }
                else if (Dragging && !Mouse.current.leftButton.isPressed)
                {
                    LevelManager.UpdateInstance(target);

                    Dragging = false;
                    copied = false;
                }
                else if (Mouse.current.rightButton.isPressed && target.name != "Player" && target.name != "Exit")
                {
                    Dragging = false;
                    if (Selected == target)
                        Selected = null;

                    LevelManager.RemoveInstance(target);
                    target = null;
                }
            }
            else if (Mouse.current.leftButton.isPressed)
                LevelManager.AddInstance(SourceName, frameSprite.transform.position, true);
        }

        public void ClearInput()
        {
            Dragging = false;
            copied = false;
            Selected = null;
        }
    }
}
