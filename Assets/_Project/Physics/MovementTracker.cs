using UnityEngine;
using ENA.Utilities;
using Event = ENA.Utilities.Event;

namespace ENA.Physics
{
    [RequireComponent(typeof(CharacterController))]
    public class MovementTracker: MonoBehaviour
    {
        #region Variables
        float analogPower;
        float timeLeft = 0;
        Vector3 startingSpot, forward;
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
            if (timeLeft > 0.001f) {
                timeLeft -= Time.fixedDeltaTime;
                Walk();
            } else if (IsWalking) {
                EndWalking();
            }
        }

        private void Start()
        {
            characterController = GetComponent<CharacterController>();

            IsWalking = false;
            startingSpot = transform.position;
        }
        #endregion
        #region Methods
        public void BeginWalking(float value, float time, bool countStep = true)
        {
            if(IsWalking) return;

            analogPower = value;
            if ((transform.position - startingSpot).magnitude > 0.9f) {
                startingSpot = transform.position;
            }
            forward = transform.forward;

            IsWalking = true;
            timeLeft = time;

            OnBeginWalking.Invoke();
        }

        private void EndWalking()
        {
            IsWalking = false;

            OnEndWalking.Invoke();
        }

        public void RevertWalk()
        {
            transform.position = startingSpot;
            timeLeft = 0;
            IsWalking = false;
        }

        private void Walk()
        {
            var delta = analogPower * transform.forward * Time.fixedDeltaTime * speed;
            characterController.Move(delta);
        }
        #endregion
    }
}