using UnityEngine;
using ENA.Utilities;
using Event = ENA.Utilities.Event;

namespace ENA.Physics
{
    [RequireComponent(typeof(CharacterController))]
    public class MovementTracker: ExtendedMonoBehaviour
    {
        #region Variables
        float timeLeft = 0;
        Vector3 startingSpot, targetSpot;
        [SerializeField] float speed = 1;
        [Header("References")]
        [SerializeField] CharacterController characterController;
        #endregion
        #region Properties
        public bool IsWalking {get; private set;}
        #endregion
        #region Events
        public Event OnBeginWalking, OnEndWalking;
        #endregion
        #region MonoBehaviour Lifecycle
        private void FixedUpdate()
        {
            timeLeft -= Time.fixedDeltaTime;

            if (timeLeft > 0f) {
                Walk(Time.fixedDeltaTime * speed);
            } else if (IsWalking) {
                EndWalking();
            }
        }

        private void Start()
        {
            characterController = GetComponent<CharacterController>();

            IsWalking = false;
            startingSpot = Vector3.negativeInfinity;
        }
        #endregion
        #region Methods
        public void BeginWalking(float moveDistance, float time, bool countStep = true)
        {
            if(IsWalking) return;

            var currentPosition = Transform.position;
            if ((currentPosition - startingSpot).magnitude <= 0.9f) return;

            IsWalking = true;
            timeLeft = time;
            startingSpot = currentPosition;
            targetSpot = currentPosition + Transform.forward * moveDistance;

            if (countStep) {
                OnBeginWalking.Invoke();
            }
        }

        private void EndWalking()
        {
            IsWalking = false;
            timeLeft = 0;

            OnEndWalking.Invoke();
        }

        public void RevertWalk()
        {
            Transform.position = startingSpot;
            timeLeft = 0;
            IsWalking = false;
        }

        private void Walk(float deltaDistance)
        {
            characterController.MoveTowards(targetSpot, deltaDistance);
        }
        #endregion
    }
}