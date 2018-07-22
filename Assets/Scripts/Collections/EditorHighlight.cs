using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class EditorHighlight : MonoBehaviour
{
    private void Awake() =>
        SetAnimation();

    private void Update()
    {
        TargetGrid();
        DisplayInfo();

        if (!EngineUtility.IsOverUI)
            ProcessMouse();
    }

    #region gameplay

    [Header("Gameplay")]
    private bool dragging;
    private bool Dragging
    {
        get
        {
            return dragging;
        }
        set
        {
            dragging = value;
            animation[0].Play(!Dragging);
        }
    }

    private int layer;
    public int Layer
    {
        get
        {
            return layer;
        }
        set
        {
            layer = value;
            ClearInput();
        }
    }

    public int Id { get; set; }

    #region targeting

    [SerializeField]
    public KeyVariable copyKey;
    [SerializeField]
    private CaptionDisplay caption;

    [Space(10)]
    [SerializeField]
    private SerializableObjectEvent onSelect;

    private SerializableObject selected;
    private SerializableObject Selected
    {
        get
        {
            return selected;
        }
        set
        {
            selected = value;

            onSelect.Invoke(Selected);
            animation[1].Play(Selected);
        }
    }

    private SerializableObject target;
    private bool copied;

    private void TargetGrid()
    {
        Vector3 position = Vector3Int.RoundToInt(EditorGrid.Clamp(EditorGrid.MousePosition()));
        Collider2D collider = Physics2D.OverlapPoint(position, 1 << Layer);

        if (Dragging)
        {
            if (!collider)
            {
                if (Input.GetKey(copyKey) && !copied)
                    if (target.Id < 2)
                        caption.Show("Cannot copy this object.");
                    else
                    {
                        copied = true;

                        SerializableObject copy =
                            LevelManager.Main.AddInstance(target.Id, target.transform.position, true);
                        LevelManager.Main.CopyInstance(target, copy);
                    }

                target.transform.position = position;
            }
        }
        else
            target = collider ? collider.GetComponent<SerializableObject>() : null;

        bodySprite.transform.position = target ? target.transform.position : position;
        if (Selected)
            frameSprite.transform.position = Selected.transform.position;
    }

    #endregion

    #region info

    [Space(10)]
    [SerializeField]
    private Text positionText;
    [SerializeField]
    private Text layerText;
    [SerializeField]
    private Text objectText;

    private void DisplayInfo()
    {
        positionText.text = $"{bodySprite.transform.position.x}, {bodySprite.transform.position.y}";
        layerText.text = LayerMask.LayerToName(Layer);
        objectText.text = target ? $"<b>{target.name}</b>" : LevelManager.Main.Source(Id).name;
    }

    #endregion

    private void ProcessMouse()
    {
        if (target)
        {
            if (!Dragging && Input.GetMouseButtonDown(0))
            {
                Dragging = true;
                Selected = Selected == target ? null : target;
            }
            else if (Dragging && !Input.GetMouseButton(0))
            {
                LevelManager.Main.UpdateInstance(target);

                Dragging = false;
                copied = false;
            }
            else if (Input.GetMouseButton(1))
                if (target.Id < 2)
                    caption.Show("Cannot delete this object.");
                else
                {
                    Dragging = false;
                    if (Selected == target)
                        Selected = null;

                    LevelManager.Main.RemoveInstance(target);
                    target = null;
                }
        }
        else if (Input.GetMouseButton(0))
            LevelManager.Main.AddInstance(Id, bodySprite.transform.position, true);
    }

    public void ClearInput()
    {
        Dragging = false;
        copied = false;
        Selected = null;
    }

    #endregion

    #region animation

    [Header("Animation")]
    [SerializeField]
    private SpriteRenderer bodySprite;
    [SerializeField]
    private SpriteRenderer frameSprite;

    private new Sequence[] animation;

    private void SetAnimation() => 
        animation = new Sequence[]
        {
            DOTween.Sequence().Insert
            (
                bodySprite.material
                    .DOFade(bodySprite.material.color.a * 2, .15f)
            ),
            DOTween.Sequence().Insert
            (
                frameSprite.material
                    .DOFade(0, .15f)
            ),
        };

    #endregion
}