using UnityEngine;

namespace ENA.Utilities
{
    [AddComponentMenu("ENA/Pressure Plate")]
    public class PressurePlateComponent: MonoBehaviour
    {
        #region Variables
        public Event OnPress;
        #endregion
        #region Methods
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player") {
                OnPress.Invoke();
            }
        }
        #endregion
    }
}