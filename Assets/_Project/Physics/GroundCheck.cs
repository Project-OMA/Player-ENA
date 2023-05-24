using ENA.Maps;
using UnityEngine;

namespace ENA.Physics
{
    [RequireComponent(typeof(Collider))]
    public partial class GroundCheck: MonoBehaviour
    {
        #region Variables
        [SerializeField] CollisionTracker collisionTracker;
        #endregion
        #region MonoBehaviour Lifecycle
        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponentInParent(out FloorComponent floor)) {
                collisionTracker.HitFloor(floor);
            }
        }
        #endregion
    }
}