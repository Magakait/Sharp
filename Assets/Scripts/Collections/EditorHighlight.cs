using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class EditorHighlight : MonoBehaviour
{
    private void Awake() =>
        animation = gameObject.AddComponent<TweenArrayComponent>().Init
        (
            DOTween.Sequence().Insert
            (
                frameSprite
                    .DOFade(1, .15f)
            ),
            DOTween.Sequence().Insert
            (
                selectionSprite
                    .DOFade(0, .15f)
            )
        );

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
                if (Input.GetKey(copyKey) && !copied && target.Id > 1)
                {
                    copied = true;

                    SerializableObject copy = LevelManager.AddInstance(target.Id, target.transform.position, true);
                    LevelManager.CopyInstance(target, copy);
                }

                target.transform.position = position;
            }
        }
        else
            target = collider ? collider.GetComponent<SerializableObject>() : null;

        frameSprite.transform.position = target ? target.transform.position : position;
        if (Selected)
            selectionSprite.transform.position = Selected.transform.position;
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
        positionText.text = $"{frameSprite.transform.position.x}, {frameSprite.transform.position.y}";
        layerText.text = LayerMask.LayerToName(Layer);
        objectText.text = target ? $"<b>{target.name}</b>" : LevelManager.Source(Id).name;
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
                LevelManager.UpdateInstance(target);

                Dragging = false;
                copied = false;
            }
            else if (Input.GetMouseButton(1) && target.Id > 1)
            {
                Dragging = false;
                if (Selected == target)
                    Selected = null;

                LevelManager.RemoveInstance(target);
                target = null;
            }
        }
        else if (Input.GetMouseButton(0))
            LevelManager.AddInstance(Id, frameSprite.transform.position, true);
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
    private SpriteRenderer frameSprite;
    [SerializeField]
    private SpriteRenderer selectionSprite;

    private new TweenArrayComponent animation;

    #endregion
}