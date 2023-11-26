using ENA.Maps;
using UnityEngine;

namespace ENA.Goals
{
    public partial class ObjectiveComponent: MonoBehaviour
    {
        #region Variables
        [SerializeField] CollidableProp propComponent;
        [SerializeField] AudioSource loopingSource;
        #endregion
        #region MonoBehaviour Lifecycle
        void Awake()
        {
            if (loopingSource == null) CaptureAudioSource();
            if (propComponent != null) name = propComponent.Prop.Name;
        }
        /// <summary>
        /// Reset is called when the user hits the Reset button in the Inspector's
        /// context menu or when adding the component the first time.
        /// </summary>
        void Reset()
        {
            TryGetComponent(out propComponent);
            CaptureAudioSource();
        }
        #endregion
        #region Methods
        private void CaptureAudioSource()
        {
            if (gameObject.TryGetComponentInChildren(out AudioSource source)) {
                source.TryGetComponent(out loopingSource);
            } else {
                TryGetComponent(out loopingSource);
            }
        }

        public string ExtractObjectiveName()
        {
            return transform.ExtractProp(out Prop prop) ? prop.Name : "Objective";
        }

        public void PlayCollisionSound()
        {
            if (propComponent != null) propComponent.CollisionAudioSource.RequestPlay();
        }

        public void PlaySound()
        {
            if (loopingSource != null) loopingSource.Play();
        }

        public void PlaySoundDelayed(float time)
        {
            if (loopingSource != null) loopingSource.PlayDelayed(time);
        }

        public void StopSound()
        {
            if (loopingSource != null) loopingSource.Stop();
        }
        #endregion
    }
}