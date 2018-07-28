using UnityEngine;

using DG.Tweening;
using Newtonsoft.Json.Linq;

public static class Extensions
{
    public static void Emission(this ParticleSystem particleSystem, bool enabled)
    {
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        emission.enabled = enabled;
    }

    public static void Refresh(this ParticleSystem particleSystem)
    {
        bool playing = particleSystem.isPlaying;
        particleSystem.Simulate(Time.time);
        if (playing)
            particleSystem.Play();
    }

    public static void Clear(this Transform transform)
    {
        foreach (Transform child in transform)
            Object.Destroy(child.gameObject);

        transform.DetachChildren();
    }

    public static Tween Play(this Tween tween, bool isBackwards)
    {
        tween.isBackwards = isBackwards;
        tween.Play();

        return tween;
    }

    public static Sequence Insert(this Sequence sequence, params Tween[] tweens)
    {
        foreach (Tween i in tweens)
            sequence.Insert(0, i);

        return sequence;
    }

    public static Vector3 ToVector(this JToken token) => 
        new Vector3((int)token["x"], (int)token["y"]);

    public static JToken ToJToken(this Vector2 vector)
    {
        vector = Vector2Int.RoundToInt(vector);
        return new JObject()
        {
            ["x"] = (int)vector.x,
            ["y"] = (int)vector.y
        };
    }

    public static JToken ToJToken(this Vector3 vector) => 
        ((Vector2)vector).ToJToken();

    public static Color Fade(this Color color, float alpha = 0)
    {
        color.a = alpha;
        return color;
    }
}