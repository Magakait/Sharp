using UnityEngine;

public class PlayerObject : SerializableObject
{
    private void Awake() =>
        CameraManager.Position = transform.position;

    private void Start() =>
        CameraManager.Target = transform;

    private void Update()
    {
        Read();
        Move();
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

    private int direction = -1;
    private bool press;

    private void Read()
    {
        bool sprint = Input.GetKey(sprintKey);
        for (int i = 0; i < 4; i++)
            if (movable.CanMove(i))
                if (Input.GetKeyDown(directionKeys[i]))
                {
                    direction = i;
                    press = true;
                    break;
                }
                else if (!press && sprint && !movable.IsMoving && Input.GetKey(directionKeys[i]))
                    direction = i;
    }

    private void Move()
    {
        if (direction >= 0 && !movable.IsMoving && movable.CanMove(direction))
        {
            movable.Move(direction);

            press = false;
            direction = -1;
        }
    }

    public void CheckSpawn()
    {
        if (ExitObject.Passed)
            return;

        if (spawn)
            spawn.StartCoroutine(spawn.Spawn());
        else
            Instantiate(message, movable.IntPosition, Quaternion.identity)
                .Setup("- - - -", "Restart", () => EngineUtility.Main.OpenScene());
    }

    #endregion
}