using System;
using System.Collections;
using System.Collections.Generic;
using ENA.Goals;
using ENA.Persistency;
using ENA.Physics;
using ENA.Player;
using ENA.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace ENA.Input
{
    [RequireComponent(typeof(PlayerComponent), typeof(CollisionTracker))]
    [RequireComponent(typeof(MovementTracker), typeof(RotationTracker))]
    public class PlayerController: ExtendedMonoBehaviour
    {
        #region Variables
        [Header("Dependencies")]
        [SerializeField] PlayerComponent playerComponent;
        [SerializeField] MovementTracker movementTracker;
        [SerializeField] RotationTracker rotationTracker;
        [SerializeField] CollisionTracker collisionTracker;
        [Header("References")]
        [SerializeField] PathManager pathManager;
        [SerializeField] GameFlag isPausedFlag;
        #endregion
        #region MonoBehaviour Lifecycle
        private void OnControllerColliderHit(ControllerColliderHit col)
        {
            GameObject collidedObject = col.gameObject;
            collisionTracker.HandleCollision(collidedObject);
        }
        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {
            rotationTracker.OnTurn -= playerComponent.BeepDirection;
            collisionTracker.OnHitFloor -= playerComponent.PlaySoundStep;
            collisionTracker.OnHitObstacle -= WalkBack;
            collisionTracker.OnHitObjective -= WalkBack;
            isPausedFlag.OnChangeValue -= SetActive;
        }
        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        void OnDisable()
        {
            movementTracker.enabled = false;
            rotationTracker.enabled = false;
        }
        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            movementTracker.enabled = true;
            rotationTracker.enabled = true;
        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        IEnumerator Start()
        {
            rotationTracker.OnTurn += playerComponent.BeepDirection;
            collisionTracker.OnHitFloor += playerComponent.PlaySoundStep;
            collisionTracker.OnHitObstacle += WalkBack;
            collisionTracker.OnHitObjective += WalkBack;
            isPausedFlag.OnChangeValue += SetActive;

            yield return new WaitForSeconds(0.5f);

            pathManager.NewPath(gameObject);
        }
        /// <summary>
        /// Reset is called when the user hits the Reset button in the Inspector's
        /// context menu or when adding the component the first time.
        /// </summary>
        void Reset()
        {
            TryGetComponent(out playerComponent);
            TryGetComponent(out movementTracker);
            TryGetComponent(out rotationTracker);
            TryGetComponent(out collisionTracker);
        }
        #endregion
        #region Methods
        public void SetActive(bool value)
        {
            this.enabled = value;
        }

        public void ToggleControls()
        {
            this.enabled = !this.enabled;
        }

        private void WalkBack(GameObject gameObject)
        {
            movementTracker.RevertWalk();
        }

        private void WalkBack(ObjectiveComponent objective)
        {
            movementTracker.RevertWalk();
        }
        #endregion
    }
}
