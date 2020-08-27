using System.Collections.Generic;
using UnityEngine;
using Sharp.Core;
using Newtonsoft.Json.Linq;

namespace Sharp.Gameplay
{
    [RequireComponent(typeof(Animator))]
    public class SwitcherObject : MonoBehaviour, ISerializable
    {
        [Header("Gameplay")]
        [SerializeField] private bool enter;
        public bool Enter
        {
            get => enter;
            set
            {
                enter = value;
                animator.SetBool("Enter", Enter);
            }
        }
        [SerializeField] private bool exit;
        public bool Exit
        {
            get => exit;
            set
            {
                exit = value;
                animator.SetBool("Exit", Exit);
            }
        }
        [SerializeField] private Vector2 origin;
        [SerializeField] private Vector2 offset;

        [Space(10)]
        [SerializeField] private Transform effectTransform;
        [SerializeField] private ParticleScalerComponent[] particleScalers;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            effectTransform.parent = null;
        }

        private void Start() =>
            PhysicsUtility.OverlapBox
            (
                targets,
                effectTransform.position,
                effectTransform.localScale,
                Constants.CellMask
            );

        private void OnDestroy()
        {
            if (effectTransform)
                Destroy(effectTransform.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!Enter)
                return;

            Switch(true);
            animator.SetTrigger("StepIn");
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            if (!Exit)
                return;

            Switch(false);
            animator.SetTrigger("StepOut");
        }

        private readonly List<StateComponent> targets = new List<StateComponent>();

        private void Place(Vector2 origin, Vector2 offset)
        {
            this.origin = origin;
            this.offset = offset;

            effectTransform.position = origin + .5f * offset;
            effectTransform.localScale = Vector2.one + new Vector2(Mathf.Abs(offset.x), Mathf.Abs(offset.y));

            foreach (var scaler in particleScalers)
                scaler.Scale();
        }

        private void Switch(bool isUp)
        {
            int delta = isUp ? 1 : -1;
            targets.ForEach(i => i.State += delta);
        }

        public void Serialize(JToken token)
        {
            token["enter"] = Enter;
            token["exit"] = Exit;

            token["origin"] = origin.ToJToken();
            token["offset"] = offset.ToJToken();
        }

        public void Deserialize(JToken token)
        {
            Enter = (bool)token["enter"];
            Exit = (bool)token["exit"];

            Place(token["origin"].ToVector(), token["offset"].ToVector());
        }
    }
}
