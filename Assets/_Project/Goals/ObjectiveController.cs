using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;
using ENA.Input;
using UnityEngine.Serialization;
using UnityEngine.Localization.Settings;

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
        public int NumberOfObjectives => objectives?.Count ?? 0;
        public GameObject NextObjective {
            get {
                if (NumberOfObjectives > 0) return objectives[0];
                else return default;
            }
        }
        #endregion
        [SerializeField] InitAudios initAudios;

        #region Methods
        #endregion
        void AdicionarListaColisions()
        {
            for (int i = 0; i < objectives.Count + 1; i++)
            {
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
            else
            {
                StartCoroutine(StartSoundObjective(1, 5));
                return true;
            }
        }

        public void MoveToNextObjective()
        {
            objectives.RemoveAt(0);
        }

        public void PlayFindObjective()
        {
            initAudios.PlayAudioFoundObjective();
        }

        public void StopObjectiveAudio()
        {
            Debug.Log("Ativou StopObjectiveAudio");

            if (objectives.Count > 0)
            {
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

        public void saveUserStatus()
        {
            var sceneName = SceneManager.GetActiveScene().name;;
            string path = Application.persistentDataPath + '/' + ControleMenuPrincipal.NomeDoUsuario + "_" + sceneName + "_timeLog.txt";

            //Write some text to the test.txt file
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(" ");

            System.DateTime currentTime = System.DateTime.Now;

            string time = currentTime.Hour + ":" + currentTime.Minute;
            writer.WriteLine("Usuario: " + ControleMenuPrincipal.NomeDoUsuario + "-" + time);
            writer.WriteLine(" ");

            writer.WriteLine("Tempo Total: " + UserModel.time);

            for (int i = 0; i < UserModel.parcialTime.Count; i++)
            {
                writer.WriteLine("T" + i + ": " + UserModel.parcialTime[i]);
            }


            writer.WriteLine(" ");

            writer.WriteLine("Numero de colisoes: " + UserModel.colisions);
            //writer.WriteLine("Ajudas: " + UserModel.helps);

            writer.Close();
        }

        public void saveInfos()
        {
            saveUserStatus();
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