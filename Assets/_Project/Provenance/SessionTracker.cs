using ENA.Goals;
using ENA.Services;
using UnityEngine;
using Event = ENA.Event;

namespace ENA.Provenance
{
    public class SessionTracker: MonoBehaviour
    {
        #region Constants
        private const string AppToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpYXQiOjE2NTE3ODYwODAsInN1YiI6ImE1NjVmOTZiLTUyODYtNDU0My04NDM2LTI2MGNhYzYzNTQ3ZiJ9.Ku4YSdVl69ynPjXRCuir0gtvVV6buobtSuoui1SK59Y";
        private const string DevGroupID = "51799";
        #endregion
        #region Variables
        [field: SerializeField] public SessionModel Model {get; private set;}
        [Header("Micelio Settings")]
        [SerializeField] bool enableAPI;
        [SerializeField] bool devMode;
        MicelioWebService micelioWeb;
        [Header("References")]
        [SerializeField] Transform playerTransform;
        [SerializeField] ObjectiveList objectiveList;
        [SerializeField] SettingsProfile profile;
        Vector3 lastPosition;
        [Header("Events")]
        public Event OnSessionStarted;
        public Event<bool> OnSessionEnded;
        public Event OnObjectiveCompleted;
        public Event<string> OnCollision;
        #endregion
        #region MonoBehaviour Lifecycle
        private void Awake()
        {
            micelioWeb = new(AppToken, devMode, enableAPI);
        }
        #endregion
        #region Methods
        public void CloseSession()
        {
            Model.GenerateResults(false);
            OnSessionEnded.Invoke(objectiveList.ClearedAllObjectives); // speaker.SpeakActivityResults(objectiveList.ClearedAllObjectives);
            micelioWeb.Disable();
        }

        public void ClearSession()
        {
            Model.GenerateResults(true);
        }

        public void OpenSession()
        {
            const int mapID = -1;

            Model = new SessionModel(profile.LoggedProfile?.UserID ?? -1, mapID);
            RegisterNextObjective();
            lastPosition = playerTransform.position;

            micelioWeb.OpenSession("Activity Started!", DevGroupID, mapID.ToString());

            #if ENABLE_LOG
            Debug.Log("Started Session!");
            #endif
        }

        public void RegisterCollision(GameObject gameObject)
        {
            string objectID;
            var timestamp = Time.time;
            objectID = gameObject.transform.ExtractPropID();

            var collisionModel = new Collision(objectID, timestamp, playerTransform.position);
            Model.Register(collisionModel);

            micelioWeb.Register(collisionModel.GenerateActivity(Model.CurrentObjective));
            OnCollision.Invoke(objectID); //speaker.SpeakCollision(objectID);

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
            var objectiveID = gameObject.transform.ExtractPropID();
            var objective = new Objective(objectiveID, "");
            Model.Register(objective);

            #if ENABLE_LOG
            Debug.Log($"New Objective: {currentObjective.ObjectiveID}");
            #endif
        }

        public void RegisterRotation(bool turnedRight)
        {
            if (!Model.HasObjectives) return;

            var direction = turnedRight ? Direction.Basic.Right : Direction.Basic.Left;

            Action actionModel = Action.Turn(direction, playerTransform.position);
            Model.Register(actionModel);

            micelioWeb.Register(actionModel.GenerateActionActivity(Model.CurrentObjective));

            #if ENABLE_LOG
            Debug.Log($"{actionModel.Timestamp} | Player turned {actionModel.Direction} @ {actionModel.Position}.");
            #endif
        }

        public void RegisterStep()
        {
            if (!Model.HasObjectives) RegisterNextObjective();

            Vector3 currentPosition = playerTransform.position;
            Action actionModel = Action.Walk(currentPosition, lastPosition, Vector3.right);
            Model.Register(actionModel);

            micelioWeb.Register(actionModel.GenerateActionActivity(Model.CurrentObjective));

            #if ENABLE_LOG
            Debug.Log($"{actionModel.Timestamp} | Player Moved: ({lastPosition} -> {actionModel.Position}).");
            #endif
            lastPosition = currentPosition;
        }
        #endregion
    }
}