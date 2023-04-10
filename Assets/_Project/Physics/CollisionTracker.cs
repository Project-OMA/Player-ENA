using UnityEngine;
using ENA.Utilities;
using ENA.Goals;
using ENA.Maps;

namespace ENA.Physics
{
    public class CollisionTracker: MonoBehaviour
    {
        #region Variables
        [SerializeField] Transform target;
        [SerializeField] ObjectiveList objectiveList;
        #endregion
        #region Properties
        public Transform Target => target;
        #endregion
        #region Events
        public Event<GameObject> OnHitFloor, OnHitObstacle;
        public Event<ObjectiveComponent> OnHitObjective;
        #endregion
        #region MonoBehaviour Lifecycle
        private void OnControllerColliderHit(ControllerColliderHit col)
        {
            GameObject collidedObject = col.gameObject;

            if (collidedObject.TryGetComponentInParent(out FloorComponent floor)) {
                HitFloor(floor);
            } else if (collidedObject.TryGetComponentInParent(out ObjectiveComponent objective)) {
                HitObjective(objective);
            } else if (collidedObject.TryGetComponentInParent(out CollidableProp prop)) {
                HitProp(prop);
            } else {
                OnHitObstacle.Invoke(collidedObject);
            }
        }
        #endregion
        #region Methods
        private void HitFloor(FloorComponent floor)
        {
            target = floor.Target;
            OnHitFloor.Invoke(floor.gameObject);
        }

        private void HitObjective(ObjectiveComponent objective)
        {
            objectiveList.Check(objective);
            objective.PlayCollisionSound();
            OnHitObjective.Invoke(objective);
        }

        private void HitProp(CollidableProp prop)
        {
            #if UNITY_IOS
            if (ControleMenuPrincipal.vibrationValue) {
                Handheld.Vibrate();
            }
            #endif

            prop.CollisionAudioSource.RequestPlay();
            OnHitObstacle.Invoke(prop.gameObject);
        }
        #endregion
    }
}