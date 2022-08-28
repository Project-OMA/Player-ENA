using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;
using ENA.Input;
using UnityEngine.Serialization;
using UnityEngine.Localization.Settings;
using System.Linq;
using ENA.TTS;
using UnityEngine.Events;

namespace ENA.Goals
{
    public class ObjectiveController: MonoBehaviour
    {
        #region Variables
        [FormerlySerializedAs("objetives")]
        public List<GameObject> objectives = new List<GameObject>();
        #endregion
        #region Static Variables
        public static ObjectiveController instance;
        #endregion
        #region Properties
        public bool HasFinished => NumberOfObjectives == 0;
        public int NumberOfObjectives => objectives?.Count ?? 0;
        public GameObject NextObjective {
            get {
                if (NumberOfObjectives > 0) return objectives[0];
                else return default;
            }
        }
        #endregion
        #region Events
        [SerializeField] UnityEvent<bool> foundObjective;
        #endregion
        [SerializeField] InitAudios initAudios;
        #region Methods
        public void Add(GameObject objective)
        {
            objectives.Add(objective);
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
        #endregion
        void AdicionarListaColisions()
        {
            for (int i = 0; i < objectives.Count + 1; i++) {
                UserModel.parcialTime.Add(0);
            }
        }

        public bool FindObjective()
        {
            StopObjectiveAudio();
            MoveToNextObjective();
            NextObjective?.GetComponent<AudioSource>()?.Play();

            if (objectives.Count == 0)
                return false;
            else {
                StartCoroutine(StartSoundObjective(1, 5));
                return true;
            }
        }

        public void MoveToNextObjective()
        {
            objectives.RemoveAt(0);
            foundObjective?.Invoke(HasFinished);
        }

        public void PlayFindObjective()
        {
            initAudios.PlayAudioFoundObjective();
        }

        public void StopObjectiveAudio()
        {
            Debug.Log("Ativou StopObjectiveAudio");

            if (objectives.Count > 0) {
                ResonanceAudioSource audioSource = objectives[0].GetComponentInChildren<ResonanceAudioSource>();
                audioSource?.GetComponent<AudioSource>()?.Stop();
            }
        }

        public void StartObjectiveAudio(int time)
        {
            StartCoroutine(StartSoundObjective(1, time));
        }

        void Start()
        {
            instance = this;
            StartObjectiveAudio(3);
            Invoke(nameof(AdicionarListaColisions), 0.5f);
        }
        #region Coroutines
        IEnumerator StartSoundObjective(int index, int time)
        {
            yield return new WaitForSeconds(time);
            if (objectives.Count > 0) {
                ResonanceAudioSource audioSource = objectives[0].GetComponentInChildren<ResonanceAudioSource>();
                Debug.Log("Ativou o audio do objetivo");
                audioSource.GetComponent<AudioSource>().Play();
            }
        }
        #endregion
        #region Coroutines
        public IEnumerator StartAudios()
        {
            yield return LocalizationSettings.InitializationOperation;
            initAudios.PlayIntroMessage();
        }
        #endregion
    }
}