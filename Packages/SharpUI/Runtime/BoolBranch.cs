using UnityEngine;
using Sharp.Core.Events;

namespace Sharp.UI
{
    public class BoolBranch : MonoBehaviour
    {
        [SerializeField]
        private VoidEvent onTrue = null;
        [SerializeField]
        private VoidEvent onFalse = null;

        public void Check(bool value) => (value ? onTrue : onFalse).Invoke();
    }
}
