using System.Collections.Generic;
using ENA.Goals;
using ENA.Maps;
using UnityEngine;
using UnityEngine.Localization;

namespace ENA.Audio
{
    [AddComponentMenu("ENA/Accessibility/Speaker")]
    public class SpeakerComponent: MonoBehaviour
    {
        #region Constant
        public const float WaitingTimeForAudio = 5;
        #endregion
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

        public void SpeakCollision(CollidableProp element)
        {
            SpeakCollision(element.Prop.Name);
        }

        public void SpeakCollision(ObjectiveComponent objective)
        {
            SpeakCollision(objective.ExtractObjectiveName());
        }

        public void SpeakHint(ObjectiveList list)
        {
            SpeakHint(list.NextObjective.ExtractObjectiveName());
        }

        public void SpeakHint(string objectiveName)
        {
            if (string.IsNullOrEmpty(objectiveName))
                objectiveName = InitialSpotMessage.GetLocalizedString();

            Speak(HintMessage.GetLocalizedString(objectiveName));
        }

        public void SpeakIntro(ObjectiveList list)
        {
            SpeakIntro(list.AllObjectiveNames());
            list.NextObjective.PlaySoundDelayed(WaitingTimeForAudio);
        }

        public void SpeakIntro(List<string> objectives)
        {
            if (objectives == null || objectives.Count == 0) return;

            string text = "";
            objectives.RemoveAt(objectives.Count - 1);
            foreach(var objective in objectives) text += $" {objective},";

            Speak(IntroMessage.GetLocalizedString(text));
        }

        public void SpeakLoading()
        {
            Speak(LoadingMessage.GetLocalizedString());
        }

        public void SpeakObjectiveFound(ObjectiveComponent current, ObjectiveList list)
        {
            string currentObjective, nextObjective;

            switch (list.AmountLeft) {
                case 0:
                    return;
                case 1:
                    currentObjective = current.ExtractObjectiveName();
                    nextObjective = default;
                    break;
                default:
                    currentObjective = current.ExtractObjectiveName();
                    nextObjective = list.NextObjective.ExtractObjectiveName();
                    break;
            }

            SpeakObjectiveFound(currentObjective, nextObjective);
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
            // #if ENABLE_LOG
            Debug.Log("Speaker: "+text);
            // #endif
        }
        #endregion
    }
}