using UnityEngine;

namespace ENA
{
    public static partial class AudioSourceExtensions
    {
        public static void RequestPlay(this AudioSource self)
        {
            if (self.isPlaying) return;

            self.Play();
        }
    }
}