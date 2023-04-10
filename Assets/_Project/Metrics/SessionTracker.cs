using System.Collections.Generic;
using ENA.Accessibility;
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
        private const string AppToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpYXQiOjE2NjY4ODc3MDAsInN1YiI6IjBhMGFmODdmLTIzN2EtNDFhMC1hZjFjLTFiNjNhNDM2NDk5MiJ9.My-vLE7J9ftI-UFewaHSDeEiIb1ym2_gaWOZKWM-cic";
        private const string DevGroupID = "93196";
        #endregion
        #region Variables
        [field: SerializeField] public SessionModel Model {get; private set;}
        [Header("Micelio Settings")]
        [SerializeField] bool enableAPI;
        [SerializeField] bool devMode;
        MicelioWebService micelioWeb;
        [Header("References")]
        [SerializeField] PlayerController controller;
        [SerializeField] ObjectiveList objectiveList;
        [SerializeField] SettingsProfile profile;
        [SerializeField] SpeakerComponent speaker;
        Vector3 lastPosition;
        SessionModel.Objective currentObjective;
        #endregion
        #region MonoBehaviour Lifecycle
        private void Awake()
        {
            micelioWeb = new MicelioWebService(AppToken, devMode, enableAPI);
        }
        #endregion
        #region Methods
        public void CloseSession()
        {
            speaker.SpeakActivityResults(Model.ClearedMap);
            micelioWeb.Disable();
        }

        public void ClearSession()
        {
            Model.MarkAsCleared();
        }

        private SessionModel.Action CreateAction(SessionModel.Action.Type type, Direction.Basic direction)
        {
            var timestamp = GetCurrentTimestamp();
            return new SessionModel.Action(type, direction, timestamp, controller.Transform.position);
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
            RegisterNextObjective();
            lastPosition = controller.Transform.position;

            micelioWeb.OpenSession("Activity Started!", DevGroupID, mapID.ToString());

            #if ENABLE_LOG
            Debug.Log("Started Session!");
            #endif
        }

        public void RegisterCollision(GameObject gameObject)
        {
            string objectID;
            var timestamp = Time.time;
            objectID = ExtractID(gameObject.transform);

            var collisionModel = new SessionModel.Collision(objectID, timestamp, controller.Transform.position);
            Model.Register(collisionModel);

            micelioWeb.Register(MetricsUtility.GenerateCollisionActivity(collisionModel, currentObjective));
            speaker.SpeakCollision(objectID);

            #if ENABLE_LOG
            Debug.Log($"{collisionModel.Timestamp} | Collided with {collisionModel.ObjectID} @ {collisionModel.Position}");
            #endif
        }

        public void RegisterNextObjective()
        {
            var nextObjective = objectiveList.NextObjective;

            if (nextObjective == null) return;

            RegisterObjective(nextObjective.gameObject);
        }

        public void RegisterObjective(GameObject gameObject)
        {
            // Update Entity Timestamp

            var objectiveID = ExtractID(gameObject.transform);

            currentObjective = new SessionModel.Objective(objectiveID, GetCurrentTimestamp());
            Model.Register(currentObjective);

            #if ENABLE_LOG
            Debug.Log($"New Objective: {currentObjective.ObjectiveID}");
            #endif
        }

        public void RegisterRotation(bool turnedRight)
        {
            if (currentObjective == null) return;

            var direction = turnedRight ? Direction.Basic.Right : Direction.Basic.Left;

            var actionModel = CreateAction(SessionModel.Action.Type.Turn, direction);
            Model.Register(actionModel);

            micelioWeb.Register(MetricsUtility.GenerateActionActivity(actionModel, currentObjective));

            #if ENABLE_LOG
            Debug.Log($"{actionModel.Timestamp} | Player turned {actionModel.Direction} @ {actionModel.Position}.");
            #endif
        } 

        public void RegisterStep()
        {
            if (currentObjective == null) return;

            Vector3 currentPosition = controller.Transform.position;
            Vector3 currentDirection = currentPosition - lastPosition;

            var direction = Direction.DetermineDirection(Vector3.right, currentDirection);
            SessionModel.Action actionModel = CreateAction(SessionModel.Action.Type.Walk, direction);
            Model.Register(actionModel);

            micelioWeb.Register(MetricsUtility.GenerateActionActivity(actionModel, currentObjective));

            #if ENABLE_LOG
            Debug.Log($"{actionModel.Timestamp} | Player Moved: ({lastPosition} -> {actionModel.Position}).");
            #endif
            lastPosition = currentPosition;
        }
        #endregion
    }
}