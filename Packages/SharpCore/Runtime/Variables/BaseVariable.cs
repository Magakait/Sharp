using UnityEngine;

namespace AlKaitagi.SharpCore.Variables
{
    public abstract class BaseVariable<T> : ScriptableObject
    {
        [SerializeField]
        private T value;
        public T Value
        {
            get => value;
            set => this.value = value;
        }

        public static implicit operator T(BaseVariable<T> variable) =>
            variable.Value;
    }
}
