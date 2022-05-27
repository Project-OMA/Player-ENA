using System;
using System.Collections;
using System.Collections.Generic;
using ENA.Goals;
using ENA.Physics;
using ENA.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace ENA.Input
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(MovementTracker), typeof(RotationTracker))]
    [RequireComponent(typeof(CollisionTracker), typeof(Soundboard))]
    public class PlayerController: MonoBehaviour
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
        #endregion
        #region Other variables
        [Header("Parameters")]
        [FormerlySerializedAs("distPasso")]
        [SerializeField] float stepDistance;
        [FormerlySerializedAs("target")]
        [SerializeField] Transform cameraTransform;
        #endregion
        #region Properties
        public int RotationCount {
            get => rotationTracker.NumberOfRotations;
            set => rotationTracker.NumberOfRotations = value;
        }

        public int StepCount {
            get => movementTracker.NumberOfSteps;
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
                movementTracker.BeginWalking(forwardInput, stepDistance);
            }
        }

        private void CheckRotation()
        {
            if (ControleMenuPrincipal.giroscopioValue) {
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
            objectiveController.StopObjectiveAudio();
            objectiveController.PlayFindObjective();
            objectiveController.MoveToNextObjective();
            UserModel.countQualTempo++;
            if (objectiveController.objectives.Count != 0)
                objectiveController.StartObjectiveAudio(5);
        }

        public void SetDirection(float angle)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, angle, transform.localEulerAngles.z);
            rotationTracker.SetTrackedAngle(angle);
        }

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            movementTracker = GetComponent<MovementTracker>();
            rotationTracker = GetComponent<RotationTracker>();
            collisionTracker = GetComponent<CollisionTracker>();
            playerSoundboard = GetComponent<Soundboard>();

            movementTracker.onEndWalking += UpdateEndPosition;
            rotationTracker.onTurn += BeepOnTurn;
            collisionTracker.onHitFloor += PlayStepSounds;
            collisionTracker.onHitObstacle += (obj) => WalkBack();
            collisionTracker.onHitObjective += ChangeObjective;
        }

        public void SetCameraTransform(Transform t)
        {
            cameraTransform = t;
        }

        public void ToggleControls()
        {
            this.enabled = !this.enabled;
        }

        private void Update()
        {
            CheckRotation();
            CheckMovement();
        }

        private void UpdateEndPosition()
        {
            if (ControleMenuPrincipal.giroscopioValue) {
                transform.localPosition = new Vector3(transform.localPosition.x, 0.64f, transform.localPosition.z);
            } else {
                transform.position = collisionTracker.Target.transform.position;
            }
        }

        private void WalkBack()
        {
            int forwardInput = -1;
            movementTracker.BeginWalking(forwardInput, 0.1f, false);
        }
        #endregion
    }
}
