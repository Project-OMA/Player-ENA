using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace ENA.Accessibility
{
    [AddComponentMenu("ENA/Accessibility/Speaker")]
    public class SpeakerComponent: MonoBehaviour
    {
        #region Variables
        [SerializeField] LocalizedString ActivityFailMessage;
        [SerializeField] LocalizedString ActivitySuccessMessage;
        [SerializeField] LocalizedString CollisionMessage;
        [SerializeField] LocalizedString InitialSpotMessage;
        [SerializeField] LocalizedString IntroMessage;
        [SerializeField] LocalizedString LoadingMessage;
        [SerializeField] LocalizedString ObjectiveFoundMessage;
        [SerializeField] LocalizedString HintMessage;
        #endregion
        #region Methods
        public void SpeakActivityResults(bool wasSuccessful)
        {
            if (wasSuccessful) {
                Speak(ActivitySuccessMessage.GetLocalizedString());
            } else {
                Speak(ActivityFailMessage.GetLocalizedString());
            }
        }

        public void SpeakCollision(string objectName)
        {
            Speak(CollisionMessage.GetLocalizedString(objectName));
        }

        public void SpeakHint(string objectiveName)
        {
            if (string.IsNullOrEmpty(objectiveName))
                objectiveName = InitialSpotMessage.GetLocalizedString();

            Speak(HintMessage.GetLocalizedString(objectiveName));
        }

        public void SpeakIntro(List<string> objectives)
        {
            string text = "";
            foreach(var objective in objectives) text += $" {objective},";

            Speak(IntroMessage.GetLocalizedString(text));
        }

        public void SpeakLoading()
        {
            Speak(LoadingMessage.GetLocalizedString());
        }

        public void SpeakObjectiveFound(string currentObjective, string nextObjective)
        {
            if (string.IsNullOrEmpty(nextObjective))
                nextObjective = InitialSpotMessage.GetLocalizedString();

            Speak(ObjectiveFoundMessage.GetLocalizedString(currentObjective, nextObjective));
        }
        #endregion
        #region Static Methods
        public static void Speak(string text, bool canBeInterrupted = false)
        {
            UAP_AccessibilityManager.Say(text, canBeInterrupted);
            #if ENABLE_LOG
            Debug.Log("Speaker: "+text);
            #endif
        }
        #endregion
    }
}