using ENA.Audio;
using UnityEngine;

namespace ENA.Audio
{
    [AddComponentMenu("ENA/Audio/Player Soundboard")]
    public class Soundboard: MonoBehaviour, ISoundboard
    {
        #region Constants
        public const int STEP = 0;
        public const int BEEP_LEFT = 1;
        public const int BEEP_RIGHT = 2;
        #endregion
        #region Variables
        [SerializeField] AudioSource step;
        [SerializeField] AudioSource beepLeft;
        [SerializeField] AudioSource beepRight;
        #endregion
        #region ISoundboard Implementation
        public void Play(int clipID)
        {
            AudioSource source;

            switch (clipID) {
                case STEP:
                    source = step;
                    break;
                case BEEP_LEFT:
                    source = beepLeft;
                    break;
                case BEEP_RIGHT:
                    source = beepRight;
                    break;
                default:
                    return;
            }

            source?.RequestPlay();
        }
        #endregion
    }
}