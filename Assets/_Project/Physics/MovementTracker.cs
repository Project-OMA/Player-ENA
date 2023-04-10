using UnityEngine;
using ENA.Utilities;
using Event = ENA.Utilities.Event;
using UnityEngine.InputSystem;
using System.Collections;

namespace ENA.Physics
{
    [RequireComponent(typeof(CharacterController))]
    public class MovementTracker: ExtendedMonoBehaviour
    {
        #region Variables
        [SerializeField] float stepDistance = 1;
        Vector3 startingSpot, targetSpot;
        WaitForSeconds cooldownYield;
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

            var keyboard = Keyboard.current;

            if (keyboard.upArrowKey.wasPressedThisFrame) {
                WalkForward();
            } else if (keyboard.downArrowKey.wasPressedThisFrame) {
                WalkBackward();
            }
        }

        public void RevertWalk()
        {
            Transform.position = targetSpot = startingSpot;
        }

        private void UpdateMovement()
        {
            movement.MoveTowards(targetSpot);
        }

        public void WalkBackward()
        {
            WalkBy(-stepDistance);
        }

        public void WalkBy(float distance)
        {
            startingSpot = Transform.position;
            targetSpot += Transform.forward * distance;
            OnBeginWalking.Invoke();
            StartCoroutine(BlockInput());
        }

        public void WalkForward()
        {
            WalkBy(stepDistance);
        }
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