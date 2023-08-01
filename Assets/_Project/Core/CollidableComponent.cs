using UnityEngine;
using UnityEngine.Serialization;

namespace ENA
{
    [AddComponentMenu("ENA/Collidable")]
    public class CollidableComponent: MonoBehaviour
    {
        #region Variables
        [FormerlySerializedAs("OnPress")]
        public Event OnPlayerCollision;
        #endregion
        #region Methods
        /// <summary>
        /// OnCollisionEnter is called when this collider/rigidbody has begun
        /// touching another rigidbody/collider.
        /// </summary>
        /// <param name="other">The Collision data associated with this collision.</param>
        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Player") {
                InvokeCollision();
            }
        }

        public void InvokeCollision()
        {
            OnPlayerCollision.Invoke();
        }
        #endregion
    }
}