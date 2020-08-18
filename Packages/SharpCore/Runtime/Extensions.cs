using UnityEngine;
using Newtonsoft.Json.Linq;

namespace Sharp.Core
{
    public static class Extensions
    {
        public static void Emission(this ParticleSystem particleSystem, bool enabled)
        {
            var emission = particleSystem.emission;
            emission.enabled = enabled;
        }

        public static void Refresh(this ParticleSystem particleSystem)
        {
            if (particleSystem.isPlaying)
            {
                particleSystem.Clear();
                particleSystem.Stop();
                particleSystem.Play();
            }
        }

        public static void Clear(this Transform transform)
        {
            foreach (Transform child in transform)
                Object.Destroy(child.gameObject);

            transform.DetachChildren();
        }

        public static Vector3 ToVector(this JToken token) =>
            new Vector3((int)token[0], (int)token[1]);

        public static JToken ToJToken(this Vector2 vector)
        {
            vector = Vector2Int.RoundToInt(vector);
            return new JArray((int)vector.x, (int)vector.y);
        }

        public static JToken ToJToken(this Vector3 vector) =>
            ((Vector2)vector).ToJToken();

        public static Color Fade(this Color color, float alpha = 0)
        {
            color.a = alpha;
            return color;
        }
    }
}
