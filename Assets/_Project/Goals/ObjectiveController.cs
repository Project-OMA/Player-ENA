using UnityEngine;
using ENA.Audio;

namespace ENA.Goals
{
    public class ObjectiveController: MonoBehaviour
    {
        #region Variables
        [SerializeField] ObjectiveList objectiveList;
        [SerializeField] SpeakerComponent speaker;
        #endregion
        #region Events
        public Event StartedMission;
        public Event<bool> FoundObjective;
        public Event FinishedGame;
        #endregion
        #region MonoBehaviour Lifecycle
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            objectiveList.OnClearObjective += ClearedObjective;
            objectiveList.OnClearAllObjectives += FinishedMission;
        }
        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {
            objectiveList.OnClearObjective -= ClearedObjective;
            objectiveList.OnClearAllObjectives -= FinishedMission;
        }
        #endregion
        #region Methods
        private void ClearedObjective(ObjectiveComponent objective)
        {
            speaker.SpeakObjectiveFound(objective, objectiveList);
            FoundObjective.Invoke(objectiveList.ClearedAllObjectives);
        }

        private void FinishedMission()
        {
            FinishedGame.Invoke();
        }

        public void StateMission()
        {
            StartedMission.Invoke();
        }
        #endregion
    }
}