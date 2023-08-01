using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace ENA.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class MovementTracker: ExtendedMonoBehaviour
    {
        #region Variables
        [SerializeField] float stepDistance = 1;
        Vector3 startingSpot, targetSpot;
        WaitForSeconds cooldownYield;
        [SerializeField] InputActionReference moveForward, moveBackward;
        [Header("References")]
        [SerializeField] MovementComponent movement;
        #endregion
        #region Properties
        [field: SerializeField] public bool CanPerformMove {get; private set;}
        #endregion
        #region Events
        public Event OnBeginWalking;
        #endregion
        #region MonoBehaviour Lifecycle
        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        void OnDisable()
        {
            moveForward.action.Disable();
            moveBackward.action.Disable();
        }
        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            moveForward.action.Enable();
            moveBackward.action.Enable();
        }
        /// <summary>
        /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
        /// </summary>
        void FixedUpdate()
        {
            UpdateMovement();
        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            CanPerformMove = true;
            startingSpot = targetSpot = Transform.position;
            cooldownYield = new WaitForSeconds(stepDistance / movement.MoveSpeed);
        }
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            CheckInput();
        }
        #endregion
        #region Methods
        private void CheckInput()
        {
            if (!CanPerformMove) return;

            if (moveForward.action.WasPressedThisFrame()) {
                WalkForward();
            } else if (moveBackward.action.WasPressedThisFrame()) {
                WalkBackward();
            }
        }

        public void RevertWalk() => Transform.position = targetSpot = startingSpot;
        private void UpdateMovement() => movement.MoveTowards(targetSpot);
        public void WalkBackward() => WalkBy(-stepDistance);

        public void WalkBy(float distance)
        {
            startingSpot = Transform.position;
            targetSpot += Transform.forward * distance;
            OnBeginWalking.Invoke();
            StartCoroutine(BlockInput());
        }

        public void WalkForward() => WalkBy(stepDistance);
        #endregion
        #region Coroutines
        IEnumerator BlockInput()
        {
            CanPerformMove = false;
            yield return cooldownYield;
            CanPerformMove = true;
        }
        #endregion
    }
}