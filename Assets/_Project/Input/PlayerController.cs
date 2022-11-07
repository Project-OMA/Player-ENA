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
        [Header("Dependencies")]
        [SerializeField] CharacterController characterController;
        [SerializeField] MovementTracker movementTracker;
        [SerializeField] RotationTracker rotationTracker;
        [SerializeField] CollisionTracker collisionTracker;
        [SerializeField] Soundboard playerSoundboard;
        [Header("References")]
        [SerializeField] ObjectiveController objectiveController;
        [SerializeField] GyroCamera gyro;
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

        IEnumerator Start()
        {
            characterController = GetComponent<CharacterController>();
            movementTracker = GetComponent<MovementTracker>();
            rotationTracker = GetComponent<RotationTracker>();
            collisionTracker = GetComponent<CollisionTracker>();
            playerSoundboard = GetComponent<Soundboard>();

            movementTracker.OnCancelWalking += UpdateEndPosition;
            movementTracker.OnEndWalking += UpdateEndPosition;
            rotationTracker.OnTurn += BeepOnTurn;
            collisionTracker.OnHitFloor += PlayStepSounds;
            collisionTracker.OnHitObstacle += WalkBack;
            collisionTracker.OnHitObjective += ChangeObjective;
            collisionTracker.OnHitObjective += WalkBack;

            yield return new WaitForSeconds(0.5f);

            GyroAttach(false);
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
                if (profile.GyroEnabled) {
                    GyroAttach(true);
                }
            }
        }

        private void CheckRotation()
        {
            if (profile.GyroEnabled) {
                GyroRotate();
            } else {
                float axisValue = UnityEngine.Input.GetAxis("Horizontal");
                Rotate(axisValue);
            }
        }

        private void GyroAttach(bool linked)
        {
            if (linked) {
                gyro.Transform.SetParent(Transform);
            } else {
                gyro.Transform.SetParent(null);
            }
        }

        private void GyroRotate()
        {
            var gyroAngle = gyro.Transform.eulerAngles.y;
            rotationTracker.GyroRotate(gyroAngle);
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

        public void Rotate(float signal)
        {
            rotationTracker.Rotate(signal);
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
            Transform.position = collisionTracker.Target.transform.position;
            DetachCameras();
        }

        private void DetachCameras()
        {
            if (profile.GyroEnabled) {
                GyroAttach(false);
            }
        }

        private void WalkBack(GameObject gameObject)
        {
            movementTracker.RevertWalk();
        }
        #endregion
    }
}
