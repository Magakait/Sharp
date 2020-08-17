using UnityEngine;
using Sharp.Core;

public class RotatorComponent : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private Quaternion? target = null;

    public Quaternion Rotation
    {
        get => rigidbody
            ? Quaternion.Euler(0, 0, rigidbody.rotation)
            : transform.rotation;
        set
        {
            if (rigidbody)
                rigidbody.rotation = value.eulerAngles.z;
            else
                transform.rotation = value;
        }
    }

    public Vector2 Position => rigidbody
        ? rigidbody.position
        : (Vector2)transform.position;

    private void Awake() =>
        rigidbody = GetComponent<Rigidbody2D>();

    private void FixedUpdate()
    {
        if (!target.HasValue)
            return;

        Rotation = Quaternion.Lerp(Rotation, target.Value, 12.5 * Time.fixedDeltaTime);
        if (Mathf.Abs(target.Value.eulerAngles.z - Rotation.eulerAngles.z) <= 1)
            target = null;
    }

    public void Rotate(int rotation) =>
        Rotate(Constants.Rotations[rotation]);

    public void Rotate(Quaternion rotation) =>
        target = rotation;

    public void Rotate(Vector2 point) =>
        Rotate(Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, point - Position)));
}
