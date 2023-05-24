using UnityEngine;
using ENA.Utilities;
using ENA.Goals;
using ENA.Maps;

namespace ENA.Physics
{
    public class CollisionTracker: MonoBehaviour
    {
        #region Variables
        [SerializeField] ObjectiveList objectiveList;
        #endregion
        #region Properties
        [field: SerializeField] public Transform Target {get; private set;}
        #endregion
        #region Events
        public Event<GameObject> OnHitFloor, OnHitObstacle;
        public Event<ObjectiveComponent> OnHitObjective;
        #endregion
        #region Methods
        public void HandleCollision(GameObject gameObject)
        {
            if (gameObject.TryGetComponentInParent(out ObjectiveComponent objective)) {
                HitObjective(objective);
            } else if (gameObject.TryGetComponentInParent(out CollidableProp prop)) {
                HitProp(prop);
            } else {
                OnHitObstacle.Invoke(gameObject);
            }
        }

        public void HandleTrigger(GameObject gameObject)
        {
            if (!gameObject.TryGetComponentInParent(out ObjectiveComponent objective)) return;

            objectiveList.Check(objective);
        }

        public void HitFloor(FloorComponent floor)
        {
            Target = floor.Target;
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