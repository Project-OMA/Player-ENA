using UnityEngine;
using ENA.Goals;
using ENA.Maps;

namespace ENA.Player
{
    public class CollisionTracker: MonoBehaviour
    {
        #region Variables
        [SerializeField] ObjectiveList objectiveList;
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
            // Check if button was pressed
            if (gameObject.TryGetComponentInParent(out ObjectiveComponent objective)) {
                objectiveList.Check(objective);
            }
        }

        public void HitFloor(CollidableProp floor)
        {
            floor.CollisionAudioSource.RequestPlay();
            OnHitFloor.Invoke(floor.gameObject);
        }

        private void HitObjective(ObjectiveComponent objective)
        {
            objective.PlayCollisionSound();
            if (objectiveList.Check(objective)) {
                OnHitObjective.Invoke(objective);
            } else {
                OnHitObstacle.Invoke(objective.gameObject);
            }
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