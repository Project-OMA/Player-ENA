using ENA.Audio;
using ENA.Maps;
using UnityEngine;

namespace ENA.Character
{
    public partial class PlayerComponent: MonoBehaviour
    {
        #region Variables
        [SerializeField] Soundboard playerSoundboard;
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
            playerSoundboard.Play(turnedRight ? Soundboard.BEEP_RIGHT : Soundboard.BEEP_LEFT);
        }

        public void PlaySoundStep(GameObject collidedObject)
        {
            playerSoundboard.Play(Soundboard.STEP);
            if (collidedObject.TryGetComponent(out CollidableProp component)) {
                component.CollisionAudioSource?.Play();
            }
        }
        #endregion
    }
}