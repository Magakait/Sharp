using UnityEngine;

namespace Sharp.Gameplay
{
    [CreateAssetMenu(menuName = "Actions/Base")]
    public class BaseAction : ScriptableObject
    {
        [SerializeField] private Sprite shape;
        public Sprite Shape => shape;
        [SerializeField] private ParticleSystem effect;
        public ParticleSystem Effect => effect;


        public virtual void Do(PlayerObject player) { }
    }
}
