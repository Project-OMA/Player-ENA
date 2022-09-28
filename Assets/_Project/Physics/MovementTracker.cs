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
        Vector3 startingSpot;
        [SerializeField] float speed = 1;
        [Header("References")]
        [SerializeField] CharacterController characterController;
        #endregion
        #region Properties
        public bool IsWalking {get; private set;}
        public int NumberOfSteps {get; private set;}
        #endregion
        #region Events
        public Event OnBeginWalking, OnEndWalking;
        #endregion
        #region Methods
        public void BeginWalking(float value, float time, bool countStep = true)
        {
            if(IsWalking) return;

            analogPower = value;
            if ((transform.position - startingSpot).magnitude > 0.9f) {
                startingSpot = transform.position;
            }

            if (countStep) NumberOfSteps++;

            IsWalking = true;
            timeLeft = time;

            OnBeginWalking.Invoke();
        }

        private void EndWalking()
        {
            IsWalking = false;

            OnEndWalking.Invoke();
        }

        private void Start()
        {
            characterController = GetComponent<CharacterController>();

            NumberOfSteps = 0;

            IsWalking = false;
            startingSpot = transform.position;
        }

        private void FixedUpdate()
        {
            if (timeLeft > 0.001f) {
                timeLeft -= Time.fixedDeltaTime;
                Walk();
            } else if (IsWalking) {
                EndWalking();
            }
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