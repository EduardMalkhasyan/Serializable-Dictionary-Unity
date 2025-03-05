using UnityEngine;

namespace Project.Tools.Help
{
    [System.Serializable]
    public class UnityObjectWrapper<T> where T : class
    {
        [SerializeField] private Object value;

        public T Value => value as T;

        public UnityObjectWrapper(Object value)
        {
            this.value = value;
        }
    }
}
