using ENA.Physics;
using UnityEngine;

namespace ENA.Goals
{
    public partial class ObjectiveComponent: MonoBehaviour
    {
        #region Variables
        [SerializeField] CollidableProp propComponent;
        [SerializeField] AudioSource loopingSource;
        #endregion
        #region Properties
        #endregion
        #region Static Properties
        #endregion
        #region Events
        #endregion
        #region MonoBehaviour Lifecycle
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            
        }
        /// <summary>
        /// Reset is called when the user hits the Reset button in the Inspector's
        /// context menu or when adding the component the first time.
        /// </summary>
        void Reset()
        {
            TryGetComponent(out propComponent);
            GetComponentInChildren<ResonanceAudioSource>()?.TryGetComponent(out loopingSource);
        }
        #endregion
        #region Methods
        public void PlayCollisionSound()
        {
            propComponent?.CollisionAudioSource.RequestPlay();
        }

        public void PlaySound()
        {
            loopingSource?.Play();
        }
        
        public void PlaySoundDelayed(float time)
        {
            loopingSource?.PlayDelayed(time);
        }

        public void StopSound()
        {
            loopingSource?.Stop();
        }
        #endregion
        #region Operators
        #endregion
        #region Static Methods
        #endregion
    }
}