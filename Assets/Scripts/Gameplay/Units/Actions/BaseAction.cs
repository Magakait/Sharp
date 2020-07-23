using UnityEngine;

namespace Sharp.Gameplay
{
    [CreateAssetMenu(menuName = "Actions/Base")]
    public class BaseAction : ScriptableObject
    {
        [SerializeField]
        private Sprite shape;
        public Sprite Shape => shape;

        public virtual void Do(PlayerObject player) { }
    }
}
