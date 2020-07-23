using System.Linq;
using UnityEngine;
using Sharp.Managers;
using Newtonsoft.Json.Linq;

namespace Sharp.Gameplay
{
    public class ExitObject : MonoBehaviour
    {
        private void Awake() =>
            Passed = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!Passed && collision.GetComponent<PlayerObject>())
            {
                Pass();
                collision.GetComponent<UnitComponent>().Kill();
            }
        }

        public static bool Passed { get; private set; }

        private void Pass()
        {
            Passed = true;

            var passed = (JArray)SetManager.Meta["passed"];
            if (!passed.Any(t => (string)t == LevelManager.Level.ShortName))
            {
                passed.Add(LevelManager.Level.ShortName);
                SetManager.Meta.Save();
            }
        }
    }
}
