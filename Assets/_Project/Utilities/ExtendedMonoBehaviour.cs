using UnityEngine;

namespace ENA
{
    public class ExtendedMonoBehaviour: MonoBehaviour
    {
        #region Variables
        public Transform Transform {get; private set;}
        #endregion
        #region MonoBehaviour Lifecycle
        protected virtual void Awake()
        {
            Transform = transform;
        }
        #endregion
    }
}