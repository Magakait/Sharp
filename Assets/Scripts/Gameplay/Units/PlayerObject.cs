using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerObject : SerializableObject
{
    private void Awake() => CameraManager.Position = transform.position;

    private void Start() => CameraFollow.Target = transform;

    private void Update()
    {
        if (Time.timeScale == 0)
            return;

        Rotate();
        Buffer();
        if (!movable.IsMoving)
            Move();
        if (cooldownEffect.emission.enabled && actionKey.IsDown)
            StartCoroutine(Act());
    }

    [Space(10)]
    [SerializeField]
    private MovableComponent movable;
    public MovableComponent Movable => movable;

    [SerializeField]
    private new Collider2D collider;
    public Collider2D Collider => collider;

    [SerializeField]
    private Prompt prompt;

    [Space(10)]
    [SerializeField]
    private KeyVariable sprintKey;
    [SerializeField]
    private KeyVariable[] directionKeys;
    [SerializeField]
    private KeyVariable[] rotationKeys;
    [SerializeField]
    private KeyVariable actionKey;

    [Space(10)]
    [SerializeField]
    private BaseMovement movement;
    public BaseMovement Movement
    {
        get
        {
            return movement;
        }
        set
        {
            movement = value;
            icon.sprite = Movement.Icon;
            Instantiate(assignEffect, transform.position, Quaternion.identity);
        }
    }

    [SerializeField]
    private BaseAction action;
    public BaseAction Action
    {
        get
        {
            return action;
        }
        set
        {
            action = value;
            shape.sprite = Action.Shape;
            cooldownEffect.Emission(Action.name != "Base");
            Instantiate(assignEffect, transform.position, Quaternion.identity);
        }
    }

    [SerializeField]
    private float cooldown;
    public float Cooldown
    {
        get
        {
            return cooldown;
        }
        set
        {
            cooldown = value;
        }
    }

    private CheckpointObject checkpoint;
    public CheckpointObject Checkpoint
    {
        get
        {
            return checkpoint;
        }
        set
        {
            checkpoint = value;
            Instantiate(assignEffect, transform.position, Quaternion.identity);
        }
    }

    [Space(10)]
    [SerializeField]
    private SpriteRenderer icon;
    [SerializeField]
    private SpriteRenderer shape;
    [SerializeField]
    private ParticleSystem assignEffect;
    [SerializeField]
    private ParticleSystem actionEffect;
    [SerializeField]
    private ParticleSystem cooldownEffect;

    private readonly List<int> moves = new List<int>();

    private void Buffer()
    {
        var sprint = Keyboard.current[sprintKey].isPressed;
        var stopSprint = Keyboard.current[sprintKey].wasReleasedThisFrame;

        for (var i = 0; i < 4; i++)
            if (directionKeys[i].IsDown)
            {
                moves.Remove(i);
                moves.Add(i);
                break;
            }
            else if (sprint && Keyboard.current[directionKeys[i]].isPressed)
            {
                moves.Remove(i);
                moves.Add(i);
            }
            else if (stopSprint || (sprint && Keyboard.current[directionKeys[i]].wasReleasedThisFrame))
                moves.Remove(i);
    }

    private void Rotate()
    {
        if (rotationKeys[0].IsDown)
            movable.Direction--;
        else if (rotationKeys[1].IsDown)
            movable.Direction++;
    }

    private void Move()
    {
        bool moved = false;

        foreach (var i in moves)
            if (movable.CanMove(i))
            {
                movement.Move(movable, i);
                moved = true;
                break;
            }

        if (!moved)
            movement.Idle(movable);

        moves.Clear();
    }

    private IEnumerator Act()
    {
        action.Do(this);
        cooldownEffect.Emission(false);
        Instantiate(actionEffect, transform.position, Constants.Rotations[movable.Direction]);

        yield return new WaitForSeconds(Cooldown);
        cooldownEffect.Emission(true);
    }

    public void CheckSpawn()
    {
        if (ExitObject.Passed)
            Instantiate(this.prompt, movable.Position, Quaternion.identity)
                .Setup("Home", () => EngineUtility.Main.LoadScene("Home"));
        else if (Checkpoint)
            Checkpoint.StartCoroutine(Checkpoint.Spawn());
        else
            Instantiate(this.prompt, movable.Position, Quaternion.identity)
                .Setup("Restart", () => EngineUtility.Main.ReloadScene());
    }
}
