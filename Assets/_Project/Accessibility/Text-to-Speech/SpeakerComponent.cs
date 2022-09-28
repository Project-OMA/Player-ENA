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
            Debug.Log("Speaker: "+text);
        }
        #endregion
    }
}