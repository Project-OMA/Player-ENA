using ENA.Audio;
using UnityEngine;

namespace ENA.Player
{
    [AddComponentMenu("ENA/Player/Player Soundboard")]
    public class Soundboard: MonoBehaviour, ISoundboard
    {
        #region Variables
        [SerializeField] AudioSource step;
        [SerializeField] AudioSource beepLeft;
        [SerializeField] AudioSource beepRight;
        #endregion
        #region ISoundboard Implementation
        public void Play(string audioClip)
        {
            AudioSource source;
            switch (audioClip) {
                case "Step":
                    source = step;
                    break;
                case "beepLeft":
                    source = beepLeft;
                    break;
                case "beepRight":
                    source = beepRight;
                    break;
                default:
                    return;
            }

            if (!(source?.isPlaying ?? true)) {
                source.Play();
            }
        }
        #endregion
    }
}