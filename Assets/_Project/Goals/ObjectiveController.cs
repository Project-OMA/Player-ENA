using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using System.Linq;
using ENA.Accessibility;
using ENA.Utilities;
using Event = ENA.Utilities.Event;
using ENA.Physics;
using UnityEngine.Serialization;
using System;

namespace ENA.Goals
{
    public class ObjectiveController: MonoBehaviour
    {
        #region Constant
        public const float WaitingTimeForAudio = 5;
        #endregion
        #region Variables
        public List<GameObject> objectives = new List<GameObject>();
        [SerializeField] SpeakerComponent speaker;
        [SerializeField] GameObject startingPoint;
        #endregion
        #region Static Variables
        public static ObjectiveController instance;
        #endregion
        #region Properties
        public bool ClearedAllObjectives => NumberOfObjectives == 0;
        public int NumberOfObjectives => objectives.Count;
        public GameObject NextObjective {
            get {
                if (NumberOfObjectives > 0) return objectives[0];
                else return default;
            }
        }
        #endregion
        #region Events
        public Event<bool> FoundObjective;
        public Event FinishedGame;
        #endregion
        #region Methods
        public void Add(GameObject objective)
        {
            objectives.Add(objective);
        }

        public void EndGame()
        {
            startingPoint.SetActive(false);

            FinishedGame.Invoke();
        }

        public string ExtractObjectiveName(GameObject obj)
        {
            if (obj.TryGetComponent<CollidableProp>(out var prop))
                return prop.GetName();
            else
                return obj.name;
        }

        public bool GetObjectiveAudioSource(int objectiveIndex, out AudioSource source)
        {
            source = default;
            if (objectiveIndex >= NumberOfObjectives) return false;

            var resonance = objectives[objectiveIndex].GetComponentInChildren<ResonanceAudioSource>();
            return resonance.TryGetComponent(out source);
        }

        public void MoveToNextObjective()
        {
            SwitchAudioToNextObjective();
            SpeakNextObjective();

            objectives.RemoveAt(0);

            FoundObjective.Invoke(ClearedAllObjectives);

            if (ClearedAllObjectives) startingPoint.SetActive(true);
        }

        public void RemoveAll()
        {
            objectives.Clear();
        }

        public void SortObjectivesBy(Vector3 position)
        {
            if (NumberOfObjectives <= 0) return;

            objectives = objectives.OrderBy(
                item => Vector3.Distance(position, item.transform.position)
            ).ToList();

            float distance;
            for (int i = 0; i < NumberOfObjectives; i++) {
                GameObject currentObject = objectives[i];
                distance = Vector3.Distance(currentObject.transform.position, position);
                Debug.Log("A distância do " + currentObject.name + " para o player é " + distance);
            }
        }

        public void SpeakNextObjective()
        {
            string currentObjective, nextObjective;
            switch (NumberOfObjectives) {
                case 0:
                    return;
                case 1:
                    currentObjective = ExtractObjectiveName(objectives[0]);
                    nextObjective = default;
                    break;
                default:
                    currentObjective = ExtractObjectiveName(objectives[0]);
                    nextObjective = ExtractObjectiveName(objectives[1]);
                    break;
            }

            speaker.SpeakObjectiveFound(currentObjective, nextObjective);
        }

        private void SwitchAudioToNextObjective()
        {
            if (GetObjectiveAudioSource(0, out var oldObjective))
                oldObjective.Stop();

            if (GetObjectiveAudioSource(1, out var newObjective))
                newObjective.PlayDelayed(WaitingTimeForAudio);
        }

        void Start()
        {
            instance = this;

            startingPoint.SetActive(false);
        }
        #endregion
        #region Coroutines
        public IEnumerator StartAudios()
        {
            yield return LocalizationSettings.InitializationOperation;

            var objectiveNames = objectives.ConvertAll(ExtractObjectiveName);
            speaker.SpeakIntro(objectiveNames);

            if (GetObjectiveAudioSource(0, out var newObjective))
                newObjective.Play();
        }
        #endregion
    }
}