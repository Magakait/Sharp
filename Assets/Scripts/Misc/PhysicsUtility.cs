using System.Collections.Generic;

using UnityEngine;

using static Constants;

public static class PhysicsUtility
{
    public static T Raycast<T>(Vector2 position, int direction, int mask, float distance = Mathf.Infinity) where T : Component
    {
        Collider2D collider = Physics2D.Raycast
        (
            Vector2Int.RoundToInt(position) + Directions[direction],
            Directions[direction],
            distance,
            mask
        ).collider;
        return collider ? collider.GetComponent<T>() : null;
    }

    public static void RaycastDirections<T>(T[] array, Vector2 position, int mask) where T : Component
    {
        for (var i = 0; i < Directions.Length; i++)
            array[i] = Raycast<T>(position, i, mask);
    }

    public static T Overlap<T>(Vector2 position, int mask) where T : Component
    {
        var collider = Physics2D.OverlapCircle(Vector2Int.RoundToInt(position), .1f, mask);
        return collider ? collider.GetComponent<T>() : null;
    }

    public static void OverlapDirections<T>(T[] array, Vector2 position, int mask) where T : Component
    {
        for (var i = 0; i < Directions.Length; i++)
            array[i] = Overlap<T>(position + Directions[i], mask);
    }

    public static void OverlapArea<T>(List<T> list, Vector2 from, Vector2 to, int mask) where T : Component
    {
        list.Clear();
        foreach (var collider in Physics2D.OverlapAreaAll(from, to, mask))
        {
            var component = collider.GetComponent<T>();
            if (component != null)
                list.Add(component);
        }
    }

    public static void OverlapBox<T>(List<T> list, Vector2 position, Vector2 size, int mask) where T : Component
    {
        list.Clear();
        foreach (var collider in Physics2D.OverlapBoxAll(position, size, 0, mask))
        {
            var component = collider.GetComponent<T>();
            if (component != null)
                list.Add(component);
        }
    }
}