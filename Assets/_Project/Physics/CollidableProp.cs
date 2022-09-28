using ENA.Goals;
using UnityEngine;
using UnityEngine.Localization;

namespace ENA.Physics
{
    public class CollidableProp: MonoBehaviour
    {
        #region Variables
        [field: SerializeField] public LocalizedString LocalizedName {get; private set;}
        [field: SerializeField] public AudioSource CollisionAudioSource {get; private set;}
        #endregion
        #region Region
        public string GetName()
        {
            return LocalizedName.GetLocalizedString();
        }

        public bool IsCurrentObjective()
        {
            return gameObject == ObjectiveController.instance.NextObjective;
        }
        #endregion
    }
}