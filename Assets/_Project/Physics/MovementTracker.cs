using UnityEngine;

namespace ENA.Physics
{
    [RequireComponent(typeof(CharacterController))]
    public class MovementTracker: MonoBehaviour
    {
        #region Variables
        float analogPower;
        [SerializeField] float speed = 1;
        [Header("References")]
        [SerializeField] CharacterController characterController;
        #endregion
        #region Properties
        public bool IsWalking {get; private set;}
        public int NumberOfSteps {get; private set;}
        #endregion
        #region Delegates
        public delegate void Event();
        #endregion
        #region Events
        public event Event onBeginWalking, onEndWalking;
        #endregion
        #region Methods
        public void BeginWalking(float value, float time, bool countStep = true)
        {
            analogPower = value;
            if (countStep) NumberOfSteps++;
            IsWalking = true;
            Invoke(nameof(EndWalking), time);
            onBeginWalking?.Invoke();
        }

        private void EndWalking()
        {
            IsWalking = false;
            onEndWalking?.Invoke();
        }

        private void Start()
        {
            characterController = GetComponent<CharacterController>();

            NumberOfSteps = 0;

            Invoke(nameof(EndWalking), 0.1f);
        }

        private void Update()
        {
            if (IsWalking) {
                Walk();
            }
        }

        private void Walk()
        {
            characterController.Move(analogPower * transform.forward * Time.deltaTime * speed);
        }
        #endregion
    }
}