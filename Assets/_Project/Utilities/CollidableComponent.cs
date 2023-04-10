using UnityEngine;
using UnityEngine.Serialization;

namespace ENA.Utilities
{
    [AddComponentMenu("ENA/Collidable")]
    public class CollidableComponent: MonoBehaviour
    {
        #region Variables
        [FormerlySerializedAs("OnPress")]
        public Event OnPlayerCollision;
        #endregion
        #region Methods
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player") {
                OnPlayerCollision.Invoke();
            }
        }
        #endregion
    }
}