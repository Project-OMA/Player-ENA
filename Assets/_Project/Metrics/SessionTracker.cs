using System.Collections.Generic;
using ENA.Goals;
using ENA.Input;
using ENA.Persistency;
using ENA.Physics;
using ENA.Services;
using ENA.Utilities;
using UnityEngine;

namespace ENA.Metrics
{
    public class SessionTracker: MonoBehaviour
    {
        #region Constants
        private const string AppToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpYXQiOjE2NjQ0NjA2NjUsInN1YiI6ImFmMmIzMjM1LTE3ZTItNGE5Yy04YmU0LTkzOGVlZjYxNGE0NyJ9.ApF2QYhWjlmCX8poqoAz1Qh1rY7dNASYcqUj0N7AIW4";
        private const string DevGroupID = "58339";
        #endregion
        #region Variables
        [field: SerializeField] public SessionModel Model {get; private set;}
        [Header("Micelio Settings")]
        [SerializeField] bool enableAPI;
        [SerializeField] bool devMode;
        MicelioWebService micelioWeb;
        [Header("References")]
        [SerializeField] PlayerController controller;
        [SerializeField] ObjectiveController objectiveManager;
        [SerializeField] SettingsProfile profile;
        Vector3 lastPosition;
        SessionModel.Objective currentObjective;
        #endregion
        #region MonoBehaviour Lifecycle
        private void Awake()
        {
            micelioWeb = new MicelioWebService(AppToken, devMode, enableAPI);
        }

        private void OnDestroy()
        {
            CloseSession();
        }
        #endregion
        #region Methods
        public void CloseSession()
        {
            Model = null;
            micelioWeb.Disable();
        }

        public void ClearSession()
        {
            Model.MarkAsCleared();
        }

        private SessionModel.Action CreateAction(SessionModel.Action.Type type, Direction.Basic direction)
        {
            var timestamp = GetCurrentTimestamp();
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

        private float GetCurrentTimestamp()
        {
            return Time.timeSinceLevelLoad;
        }

        public void OpenSession()
        {
            var mapID = -1;

            Model = new SessionModel(profile.LoggedProfile?.UserID ?? -1, mapID);
            RegisterObjective(objectiveManager.NextObjective);
            lastPosition = controller.transform.position;

            micelioWeb.OpenSession("Activity Started!", DevGroupID, mapID.ToString());

            Debug.Log("Started Session!");
        }

        public void RegisterCollision(GameObject gameObject)
        {
            string objectID;
            var timestamp = Time.time;
            objectID = ExtractID(gameObject.transform);

            var collisionModel = new SessionModel.Collision(objectID, timestamp, controller.transform.position);
            Model.Register(collisionModel);

            micelioWeb.Register(MetricsUtility.GenerateCollisionActivity(collisionModel, currentObjective));

            Debug.Log($"{collisionModel.Timestamp} | Collided with {collisionModel.ObjectID} @ {collisionModel.Position}");
        }

        public void RegisterObjective(GameObject gameObject)
        {
            // Update Entity Timestamp

            var objectiveID = ExtractID(gameObject.transform);

            currentObjective = new SessionModel.Objective(objectiveID, GetCurrentTimestamp());
            Model.Register(currentObjective);

            Debug.Log($"New Objective: {currentObjective.ObjectiveID}");
        }

        public void RegisterRotation(bool turnedRight)
        {
            if (currentObjective == null) return;

            var direction = turnedRight ? Direction.Basic.Right : Direction.Basic.Left;

            var actionModel = CreateAction(SessionModel.Action.Type.Turn, direction);
            Model.Register(actionModel);

            micelioWeb.Register(MetricsUtility.GenerateActionActivity(actionModel, currentObjective));

            Debug.Log($"{actionModel.Timestamp} | Player turned {actionModel.Direction} @ {actionModel.Position}.");
        } 

        public void RegisterStep()
        {
            if (currentObjective == null) return;

            Vector3 currentPosition = controller.transform.position;
            Vector3 currentDirection = currentPosition - lastPosition;

            var direction = Direction.DetermineDirection(Vector3.right, currentDirection);
            SessionModel.Action actionModel = CreateAction(SessionModel.Action.Type.Walk, direction);
            Model.Register(actionModel);

            micelioWeb.Register(MetricsUtility.GenerateActionActivity(actionModel, currentObjective));

            Debug.Log($"{actionModel.Timestamp} | Player Moved: ({lastPosition} -> {actionModel.Position}).");
            lastPosition = currentPosition;
        }
        #endregion
    }
}