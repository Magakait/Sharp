using System.Collections.Generic;

using UnityEngine;

public class PlayerObject : SerializableObject
{
    private void Awake() =>
        CameraManager.Position = transform.position;

    private void Start() =>
        CameraManager.Target = transform;

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            Read();
            Move();
        }
    }

    #region gameplay

    [Header("Gameplay")]
    public MovableComponent movable;

    [Space(10)]
    public KeyVariable sprintKey;
    public KeyVariable[] directionKeys = new KeyVariable[4];

    public ActionMessage message;
    [HideInInspector]
    public CheckpointObject spawn;

    private readonly List<int> moves = new List<int>();
    private bool press;

    private void Read()
    {
        var sprint = Input.GetKey(sprintKey);
        for (var i = 0; i < 4; i++)
            if (Input.GetKeyDown(directionKeys[i]))
            {
                press = true;

                moves.Remove(i);
                moves.Add(i);

                break;
            }
            else if (!press && sprint && Input.GetKey(directionKeys[i]))
            {
                moves.Remove(i);
                moves.Add(i);
            }
    }

    private void Move()
    {
        if (movable.IsMoving)
            return;

        moves.Reverse();
        foreach (var i in moves)
            if (movable.CanMove(i))
            {
                movable.Move(i);
                break;
            }

        moves.Clear();
        press = false;
    }

    public void CheckSpawn()
    {
        if (ExitObject.Passed)
            return;

        if (spawn)
            spawn.StartCoroutine(spawn.Spawn());
        else
            Instantiate(message, movable.IntPosition, Quaternion.identity)
                .Setup("- - - -", "Restart", () => EngineUtility.Main.LoadScene());
    }

    #endregion
}