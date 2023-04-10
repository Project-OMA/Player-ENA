using UnityEngine;

namespace ENA.Player
{
    public partial class PlayerComponent: MonoBehaviour
    {
        #region Variables
        [SerializeField] Soundboard playerSoundboard;
        #endregion
        #region Properties
        #endregion
        #region Static Properties
        #endregion
        #region Events
        #endregion
        #region MonoBehaviour Lifecycle
        /// <summary>
        /// Reset is called when the user hits the Reset button in the Inspector's
        /// context menu or when adding the component the first time.
        /// </summary>
        void Reset()
        {
            TryGetComponent(out playerSoundboard);
        }
        #endregion
        #region Methods
        public void BeepDirection(bool turnedRight)
        {
            if (turnedRight) {
                playerSoundboard.Play("beepRight");
            } else {
                playerSoundboard.Play("beepLeft");
            }
        }

        public void PlaySoundStep(GameObject collidedObject)
        {
            playerSoundboard.Play("step");
            if (collidedObject.TryGetComponent(out AudioSource source)) {
                source.Play();
            }
        }
        #endregion
        #region Operators
        #endregion
        #region Static Methods
        #endregion
    }
}