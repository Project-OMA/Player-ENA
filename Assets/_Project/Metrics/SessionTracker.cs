using System.Collections.Generic;
using ENA.Goals;
using ENA.Input;
using ENA.Persistency;
using ENA.Physics;
using ENA.Utilities;
using UnityEngine;

namespace ENA.Metrics
{
    public class SessionTracker: MonoBehaviour
    {
        #region Variables
        [field: SerializeField] public SessionModel Model {get; private set;}
        [Header("References")]
        [SerializeField] PlayerController controller;
        [SerializeField] ObjectiveController objectiveManager;
        [SerializeField] SettingsProfile profile;
        Vector3 lastPosition;
        #endregion
        #region Methods
        public void CloseSession()
        {
            Model = null;
        }

        public void ClearSession()
        {
            Model.MarkAsCleared();
        }

        private SessionModel.Action CreateAction(SessionModel.Action.Type type, Direction.Basic direction)
        {
            var timestamp = Time.time;
            return new SessionModel.Action(type, direction, timestamp, controller.transform.position);
        }

        public string ExtractID(Transform transform)
        {
            if (transform.TryGetComponent(out CollidableProp prop)) {
                return prop.LocalizedName.GetLocalizedString();
            } else {
                return "No ID";
            }
        }

        public void OpenSession()
        {
            var mapID = -1;
            Model = new SessionModel(profile.LoggedProfile?.UserID ?? -1, mapID);
            RegisterObjective(objectiveManager.NextObjective);
            lastPosition = controller.transform.position;

            Debug.Log("Started Session!");
        }

        public void RegisterCollision(GameObject gameObject)
        {
            string objectID;
            var timestamp = Time.time;
            objectID = ExtractID(gameObject.transform);

            var collisionModel = new SessionModel.Collision(objectID, timestamp, controller.transform.position);
            Model.Register(collisionModel);

            Debug.Log($"{collisionModel.Timestamp} | Collided with {collisionModel.ObjectID} @ {collisionModel.Position}");
        }

        public void RegisterObjective(GameObject gameObject)
        {
            var objectiveID = ExtractID(gameObject.transform);

            var objectiveModel = new SessionModel.Objective(objectiveID);
            Model.Register(objectiveModel);

            Debug.Log($"New Objective: {objectiveModel.ObjectiveID}");
        }

        public void RegisterRotation(bool turnedRight)
        {
            var direction = turnedRight ? Direction.Basic.Right : Direction.Basic.Left;

            var actionModel = CreateAction(SessionModel.Action.Type.Turn, direction);
            Model.Register(actionModel);

            Debug.Log($"{actionModel.Timestamp} | Player turned {actionModel.Direction} @ {actionModel.Position}.");
        } 

        public void RegisterStep()
        {
            Vector3 currentPosition = controller.transform.position;
            Vector3 currentDirection = currentPosition - lastPosition;

            var direction = Direction.DetermineDirection(Vector3.right, currentDirection);
            SessionModel.Action actionModel = CreateAction(SessionModel.Action.Type.Walk, direction);
            Model.Register(actionModel);

            Debug.Log($"{actionModel.Timestamp} | Player Moved: ({lastPosition} -> {actionModel.Position}).");
            lastPosition = currentPosition;
        }
        #endregion
    }
}