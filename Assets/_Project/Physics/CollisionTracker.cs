using UnityEngine;

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
        #region Delegates
        public delegate void Event(GameObject collidedObj);
        #endregion
        #region Events
        public event Event onHitFloor, onHitObjective, onHitObstacle;
        #endregion
        #region Methods
        private void OnControllerColliderHit(ControllerColliderHit col)
        {
            GameObject collidedObject = col.gameObject;

            if (collidedObject.tag == "floor") {
                target = collidedObject.GetComponent<GetTarget>().target;
                onHitFloor?.Invoke(collidedObject);
            } else {
                onHitObstacle?.Invoke(collidedObject);
            }

            if (collidedObject.tag == "objects")
            {
                UserModel.colisions++;

#if UNITY_IOS
                if(ControleMenuPrincipal.vibrationValue)
                {
                    Handheld.Vibrate();
                }
#endif
                print(collidedObject.name);

                collidedObject.GetComponent<objectCollider>()?.Collision();
                onHitObjective?.Invoke(collidedObject);
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