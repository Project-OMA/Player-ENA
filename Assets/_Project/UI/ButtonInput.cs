using UnityEngine;
using UnityEngine.InputSystem;

namespace ENA.UI
{
    public partial class ButtonInput: MonoBehaviour
    {
        #region Variables
        [SerializeField] InputActionReference hintInput;
        #endregion
        #region Events
        public Event RequestedHint;
        #endregion
        #region MonoBehaviour Lifecycle
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            hintInput.action.started += RequestHint;
        }
        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {
            hintInput.action.started -= RequestHint;
        }
        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        void OnDisable()
        {
            hintInput.action.Disable();
        }
        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            hintInput.action.Enable();
        }
        #endregion
        #region Methods
        public void RequestHint(InputAction.CallbackContext context)
        {
            RequestedHint.Invoke();
        }
        #endregion
    }
}