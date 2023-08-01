using UnityEngine;

namespace ENA.Player
{
    public partial class MovementComponent: MonoBehaviour
    {
        #region Variables
        [SerializeField] CharacterController controller;
        #endregion
        #region Properties
        [field: SerializeField] public float MoveSpeed {get; private set;}
        [field: SerializeField] public float SnapPosition {get; private set;}
        #endregion
        #region MonoBehaviour Lifecycle
        /// <summary>
        /// Reset is called when the user hits the Reset button in the Inspector's
        /// context menu or when adding the component the first time.
        /// </summary>
        void Reset() => controller = GetComponent<CharacterController>();
        #endregion
        #region Methods
        public void MoveTowards(Vector3 target) => MoveTowards(target, MoveSpeed * Time.deltaTime);

        public void MoveTowards(Vector3 target, float deltaDistance)
        {
            var controllerPosition = controller.transform.position;
            var targetPosition = Vector3.MoveTowards(controllerPosition, target, Mathf.Max(deltaDistance, SnapPosition));
            controller.Move(targetPosition - controllerPosition);
        }
        #endregion
    }
}