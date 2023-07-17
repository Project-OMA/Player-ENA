using UnityEngine;
using UnityEngine.InputSystem;

namespace ENA.Maps
{
    [AddComponentMenu("ENA/Move Door")]
    public class MoveDoorComponent: MonoBehaviour
    {
        #region Variables
        [field: SerializeField] public bool IsOpen {get; private set;}
        [Header("References")]
        [SerializeField] Collider doorCollider;
        [SerializeField] GameObject leftDoor;
        [SerializeField] GameObject rightDoor;
        [SerializeField] AudioSource audioSource;
        [Header("Audio Clips")]
        [SerializeField] AudioClip openDoorSound;
        [SerializeField] AudioClip closeDoorSound;
        #endregion
        #region Methods
        private void OnTriggerEnter(Collider other)
        {
            if (!IsOpen || other.name != "Player") return;

            SetDoorState(false);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.name != "Player") return;

            SetDoorState(true);
        }

        private void OnTriggerStay(Collider other)
        {
            if (IsOpen || other.name != "Player" || !Keyboard.current.spaceKey.wasPressedThisFrame) return;

            SetDoorState(false);
        }

        public void SetDoorState(bool enabled)
        {
            doorCollider.isTrigger = IsOpen;

            leftDoor.SetActive(enabled);
            rightDoor.SetActive(enabled);

            audioSource.PlayOneShot(GetClipForState(enabled));
        }

        private AudioClip GetClipForState(bool enabled)
        {
            return enabled ? closeDoorSound : openDoorSound;
        }
        #endregion
    }
}