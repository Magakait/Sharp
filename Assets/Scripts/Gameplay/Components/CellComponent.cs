using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CellComponent : MonoBehaviour
{
    [SerializeField]
    private bool hollowed;
    public bool Hollowed
    {
        get => hollowed;
        set => hollowed = value;
    }

    private void OnTriggerEnter2D(Collider2D collision) =>
        collisions.Add(collision);

    private void OnTriggerExit2D(Collider2D collision) =>
        collisions.Remove(collision);

    private readonly List<Collider2D> collisions = new List<Collider2D>();

    public IEnumerable<T> GetCollisions<T>() where T : Component =>
        collisions
            .Where(i => i)
            .Select(i => i.GetComponent<T>())
            .Where(i => i);
}
