using System;
using System.Collections;
using System.Collections.Generic;
using ENA.Goals;
using UnityEngine;
using UnityEngine.Serialization;

namespace ENA.TTS
{
    public class InitAudios: MonoBehaviour
    {
        #region Voice Lines
        public string Article1 {get; set;}
        public string Article2 {get; set;}
        public string Intro1Text {get; set;}
        public string Intro2Text {get; set;}
        public string Intro3Text {get; set;}
        public string FailedMessage {get; set;}
        public string FoundItemText {get; set;}
        public string NextObjectiveText {get; set;}
        public string ReturnToStartText {get; set;}
        public string SuccessMessage {get; set;}
        #endregion
        #region Variables
        public static int numberOfTipsGiven;
        [FormerlySerializedAs("objetiveController")]
        [SerializeField] ObjectiveController objectiveController;
        #endregion
        #region Methods
        void OnApplicationQuit()
        {
            UAP_AccessibilityManager.StopSpeaking();
        }

        public void PlayAudioFoundObjective()
        {
            string audio = "";

            if(objectiveController.objectives.Count > 1)
                audio = FoundItemText + " " + objectiveController.objectives[0].GetComponent<objectCollider>().GetName() + " " + NextObjectiveText + " " + objectiveController.objectives[1].GetComponent<objectCollider>().GetName();
            else{
                audio = FoundItemText + " " + objectiveController.objectives[0].GetComponent<objectCollider>().GetName() + " " + ReturnToStartText;
            }

            UAP_AccessibilityManager.Say(audio, false);
            print("Audio sendo tocado:" + audio);
        }

        public void PlayEndingMessage()
        {
            if (objectiveController.HasFinished) {
                UAP_AccessibilityManager.Say(SuccessMessage, false);
            } else {
                UAP_AccessibilityManager.Say(FailedMessage, false);
            }
        }

        public void PlayHintMessage()
        {
            throw new System.NotImplementedException();
        }

        public void PlayIntroMessage()
        {
            string names = "";
            string names2 = "";

            for (int i = 0; i < objectiveController.objectives.Count; i++) {
                var objectiveName = objectiveController.objectives[i].GetComponent<objectCollider>().GetName();
                names += ", " + Article1 + " " + objectiveName;
                names2 += ", " + Article2 + " " + objectiveName;
            }

            string introduction = $"{Intro1Text} {names} {Intro2Text} {names2} {Intro3Text}";
            UAP_AccessibilityManager.Say(introduction, false);
            print(introduction);
        }

        void Start()
        {
            numberOfTipsGiven = 0;
        }

        void Update()
        {
            if (!UnityEngine.Input.GetButtonDown("Jump")) return;

            string audio;
            numberOfTipsGiven++;

            if (objectiveController.NumberOfObjectives > 0) {
                var nextObjectiveName = objectiveController.objectives[0].GetComponent<objectCollider>().GetName();
                audio = NextObjectiveText + " " + nextObjectiveName;
            } else {
                audio = ReturnToStartText;
            }

            UAP_AccessibilityManager.Say(audio, false);
            print(audio);
        }
        #endregion
    }
}
