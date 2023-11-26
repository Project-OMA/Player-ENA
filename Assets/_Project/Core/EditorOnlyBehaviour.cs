using UnityEngine;

namespace ENA
{
    [DisallowMultipleComponent]
    public class EditorMonoBehaviour: MonoBehaviour
    {
        #region Variables
        [SerializeField] Component[] componentsToRemove = new Component[0];
        #endregion
        #region Properties
        [field: SerializeField] public bool DestroyGameObject {get; private set;} = true;
        [field: SerializeField] public bool DetachOnAwake {get; private set;} = false;
        [field: SerializeField] public bool DetachChildsOnDestroy {get; private set;} = false;
        #endregion
        #region MonoBehaviour Lifecycle
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            if (DetachOnAwake) transform.SetParent(null);

            DestroyComponents();

            if (!Debug.isDebugBuild) DestroyItself();
        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {
            if (DetachChildsOnDestroy) transform.DetachChildren();
        }
        #endregion
        #region Methods
        private void DestroyComponents()
        {
            foreach(var component in componentsToRemove) {
                if (component == null) continue;
                Destroy(component);
            }
        }

        private void DestroyItself()
        {
            if (DestroyGameObject) Destroy(gameObject);
            else Destroy(this);
        }
        #endregion
    }
}