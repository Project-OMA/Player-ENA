using System;
using System.Collections;
using System.Collections.Generic;
using ENA.Goals;
using ENA.Persistency;
using ENA.Physics;
using ENA.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace ENA.Input
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(MovementTracker), typeof(RotationTracker))]
    [RequireComponent(typeof(CollisionTracker), typeof(Soundboard))]
    public class PlayerController: ExtendedMonoBehaviour
    {
        #region Variables
        [Header("References")]
        [FormerlySerializedAs("_characterController")]
        [SerializeField] CharacterController characterController;
        [SerializeField] MovementTracker movementTracker;
        [SerializeField] RotationTracker rotationTracker;
        [SerializeField] CollisionTracker collisionTracker;
        [SerializeField] ObjectiveController objectiveController;
        [SerializeField] Soundboard playerSoundboard;
        [SerializeField] SettingsProfile profile;
        [Header("Parameters")]
        [SerializeField] float stepDistance;
        [SerializeField] Transform cameraTransform;
        #endregion
        #region MonoBehaviour Lifecycle
        void OnDisable()
        {
            movementTracker.enabled = false;
            rotationTracker.enabled = false;
        }

        void OnEnable()
        {
            movementTracker.enabled = true;
            rotationTracker.enabled = true;
        }

        void Start()
        {
            characterController = GetComponent<CharacterController>();
            movementTracker = GetComponent<MovementTracker>();
            rotationTracker = GetComponent<RotationTracker>();
            collisionTracker = GetComponent<CollisionTracker>();
            playerSoundboard = GetComponent<Soundboard>();

            movementTracker.OnEndWalking += UpdateEndPosition;
            rotationTracker.OnTurn += BeepOnTurn;
            collisionTracker.OnHitFloor += PlayStepSounds;
            collisionTracker.OnHitObstacle += WalkBack;
            collisionTracker.OnHitObjective += ChangeObjective;
            collisionTracker.OnHitObjective += WalkBack;
        }

        void Update()
        {
            if (!rotationTracker.IsRotating) CheckMovement();
            if (!movementTracker.IsWalking) CheckRotation();
        }
        #endregion
        #region Methods
        private void BeepOnTurn(bool turnedRight)
        {
            if (turnedRight) {
                playerSoundboard.Play("beepRight");
            } else {
                playerSoundboard.Play("beepLeft");
            }
        }
 
        private void ChangeObjective(GameObject gameObject)
        {
            if (objectiveController.NumberOfObjectives > 0 && gameObject == objectiveController.NextObjective) {
                PlayNewObjective();
            }
        }

        private void CheckMovement()
        {
            if (movementTracker.IsWalking) return;

            if (AxisTracker.VerticalDown()) {
                int forwardInput = UnityEngine.Input.GetAxis("Vertical") > 0 ? 1 : -1;
                movementTracker.BeginWalking(forwardInput, 1);
                Debug.Log($"Analog Signal: {forwardInput}");
            }
        }

        private void CheckRotation()
        {
            if (profile.GyroEnabled) {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, cameraTransform.transform.eulerAngles.y, transform.eulerAngles.z);
            } else {
                float axisValue = UnityEngine.Input.GetAxis("Horizontal");
                rotationTracker.Rotate(axisValue);
            }
        }

        private void PlayStepSounds(GameObject collidedObject)
        {
            if (!movementTracker.IsWalking) return;

            collidedObject.GetComponent<AudioSource>()?.Play();
        }

        private void PlayNewObjective()
        {
            objectiveController.MoveToNextObjective();
        }

        public void SetDirection(float angle)
        {
            Transform.localEulerAngles = new Vector3(Transform.localEulerAngles.x, angle, Transform.localEulerAngles.z);
            rotationTracker.SetTrackedAngle(angle);
        }

        public void SetCameraTransform(Transform t)
        {
            cameraTransform = t;
        }

        public void ToggleControls()
        {
            this.enabled = !this.enabled;
        }

        private void UpdateEndPosition()
        {
            if (profile.GyroEnabled) {
                Transform.localPosition = new Vector3(Transform.localPosition.x, 0.64f, Transform.localPosition.z);
            } else {
                Transform.position = collisionTracker.Target.transform.position;
            }
        }

        private void WalkBack(GameObject gameObject)
        {
            movementTracker.RevertWalk();
        }
        #endregion
    }
}
