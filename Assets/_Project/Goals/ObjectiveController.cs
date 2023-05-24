using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ENA.Accessibility;
using ENA.Utilities;
using Event = ENA.Utilities.Event;
using ENA.Physics;
using UnityEngine.InputSystem;

namespace ENA.Goals
{
    public class ObjectiveController: MonoBehaviour
    {
        #region Constant
        public const float WaitingTimeForAudio = 5;
        #endregion
        #region Variables
        [SerializeField] ObjectiveList objectiveList;
        [SerializeField] SpeakerComponent speaker;
        [SerializeField] InputActionReference hintInput;
        #endregion
        #region Events
        public Event<bool> FoundObjective;
        public Event FinishedGame;
        #endregion
        #region MonoBehaviour Lifecycle
        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        void OnDisable()
        {
            hintInput.action.Disable();
        }
        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            hintInput.action.Enable();
        }
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            hintInput.action.started += SpeakObjectiveHint;
            objectiveList.OnClearObjective += ClearedObjective;
            objectiveList.OnClearAllObjectives += FinishedMission;
        }
        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {
            hintInput.action.started -= SpeakObjectiveHint;
            objectiveList.OnClearObjective -= ClearedObjective;
            objectiveList.OnClearAllObjectives -= FinishedMission;
        }
        #endregion
        #region Methods
        private void ClearedObjective(ObjectiveComponent objective)
        {
            SpeakNextObjective(objective);
            FoundObjective.Invoke(objectiveList.ClearedAllObjectives);
        }

        public string ExtractObjectiveName(ObjectiveComponent objective)
        {
            if (objective.TryGetComponent<CollidableProp>(out var prop))
                return prop.LocalizedName?.GetLocalizedString();
            else
                return objective.name;
        }

        private void FinishedMission()
        {
            FinishedGame.Invoke();
        }

        public void StateMission()
        {
            var objectiveNames = objectiveList.Select(ExtractObjectiveName).ToList();
            speaker.SpeakIntro(objectiveNames);

            objectiveList.NextObjective.PlaySoundDelayed(WaitingTimeForAudio);
        }

        public void SpeakObjectiveHint(InputAction.CallbackContext context)
        {
            speaker.SpeakHint(ExtractObjectiveName(objectiveList.NextObjective));
        }

        public void SpeakNextObjective(ObjectiveComponent objective)
        {
            string currentObjective, nextObjective;
            switch (objectiveList.AmountLeft) {
                case 0:
                    return;
                case 1:
                    currentObjective = ExtractObjectiveName(objective);
                    nextObjective = default;
                    break;
                default:
                    currentObjective = ExtractObjectiveName(objective);
                    nextObjective = ExtractObjectiveName(objectiveList.NextObjective);
                    break;
            }

            speaker.SpeakObjectiveFound(currentObjective, nextObjective);
        }
        #endregion
    }
}