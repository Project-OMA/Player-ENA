// using Tilia.Visuals.BasicHand;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace MagicianSpace
{
    public partial class XRPlayer: MonoBehaviour
    {
        #region Enums
        public enum Movement
        {
            Continuous, Teleport
        }

        public enum Rotation
        {
            Continuous, Snap
        }
        #endregion
        #region Variables
        [Header("Parameters")]
        [field: SerializeField] Movement MovementMode;
        [field: SerializeField] Rotation RotationMode;
        [Header("References")]
        [SerializeField] TeleportationProvider teleport;
        [SerializeField] ContinuousMoveProviderBase continuousMovement;
        [SerializeField] SnapTurnProviderBase snapTurn;
        [SerializeField] ContinuousTurnProviderBase continuousTurn;
        // [field: SerializeField] public HandFacade LeftHand {get; private set;}
        // [field: SerializeField] public HandFacade RightHand {get; private set;}
        #endregion
        #region Properties
        #endregion
        #region MonoBehaviour Lifecycle
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            SetMovement(MovementMode);
            SetRotation(RotationMode);
        }
        #endregion
        #region Methods
        public void SetMovement(Movement movementType)
        {
            teleport.gameObject.SetActive(movementType == Movement.Teleport);
            continuousMovement.gameObject.SetActive(movementType == Movement.Continuous);
        }

        public void SetRotation(Rotation rotationType)
        {
            snapTurn.gameObject.SetActive(rotationType == Rotation.Snap);
            continuousTurn.gameObject.SetActive(rotationType == Rotation.Continuous);
        }
        #endregion
        #region Static Methods
        #endregion
    }
}