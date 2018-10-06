using System.Collections.Generic;

using UnityEngine;

public class PlayerObject : SerializableObject
{
    private void Awake() =>
        CameraFollow.Position = transform.position;

    private void Start() =>
        CameraFollow.Target = transform;

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            Read();
            if (!movable.IsMoving)
                Move();
        }
    }

    [Space(10)]
    [SerializeField]
    private MovableComponent movable;
    [SerializeField]
    private Prompt prompt;

    [Space(10)]
    [SerializeField]
    private KeyVariable sprintKey;
    [SerializeField]
    private KeyVariable[] directionKeys = new KeyVariable[4];

    [Space(10)]
    [SerializeField]
    private ParticleSystem effect;

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
            if (Movement != value)
            {
                Movement.Dispose(movable);
                movement = value;
                Movement.Assign(movable);

                Instantiate(effect, transform.position, Quaternion.identity);
            }
        }
    }

    [SerializeField]
    private CheckpointObject checkpoint;
    public CheckpointObject Checkpoint
    {
        get
        {
            return checkpoint;
        }
        set
        {
            if (Checkpoint != value)
            {
                checkpoint = value;
                Instantiate(effect, transform.position, Quaternion.identity);
            }
        }
    }

    private readonly List<int> moves = new List<int>();

    private void Read()
    {
        var sprint = Input.GetKey(sprintKey);
        var stopSprint = Input.GetKeyUp(sprintKey);

        for (var i = 0; i < 4; i++)
            if (Input.GetKeyDown(directionKeys[i]))
            {
                moves.Remove(i);
                moves.Add(i);
                break;
            }
            else if (sprint && Input.GetKey(directionKeys[i]))
            {
                moves.Remove(i);
                moves.Add(i);
            }
            else if (stopSprint || (sprint && Input.GetKeyUp(directionKeys[i])))
                moves.Remove(i);
    }

    private void Move()
    {
        bool moved = false;
        moves.Reverse();

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