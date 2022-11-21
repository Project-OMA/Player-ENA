using UnityEngine;
using ENA.Utilities;

namespace ENA.Physics
{
    public class CollisionTracker: MonoBehaviour
    {
        #region Variables
        [SerializeField] GameObject target;
        [SerializeField] bool closeToDoor;
        #endregion
        #region Properties
        public GameObject Target => target;
        #endregion
        #region Events
        public Event<GameObject> OnHitFloor, OnHitObjective, OnHitObstacle;
        #endregion
        #region Methods
        private void OnControllerColliderHit(ControllerColliderHit col)
        {
            GameObject collidedObject = col.gameObject;

            if (collidedObject.tag == "floor") {
                target = collidedObject.GetComponent<GetTarget>().target;
                OnHitFloor.Invoke(collidedObject);
            } else if (collidedObject.tag == "objects") {
                #if UNITY_IOS
                if (ControleMenuPrincipal.vibrationValue) {
                    Handheld.Vibrate();
                }
                #endif
                var prop = collidedObject.GetComponentInParent<CollidableProp>();
                if (prop == null) return;

                prop.CollisionAudioSource.RequestPlay();
                if (prop.IsCurrentObjective()) {
                    OnHitObjective.Invoke(prop.gameObject);
                } else {
                    OnHitObstacle.Invoke(prop.gameObject);
                }
            } else {
                OnHitObstacle.Invoke(collidedObject);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "door") {
                closeToDoor = true;
            }

            if (other.tag == "target") {
                target = other.gameObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "door") {
                closeToDoor = false;
            }
        }
        #endregion
    }
}