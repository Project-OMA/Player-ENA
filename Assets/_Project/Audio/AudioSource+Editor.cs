#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace ENA.Audio
{
    public partial class AudioSourceExtensions
    {
        [MenuItem("CONTEXT/AudioSource/Play Once")]
        private static void PlayOnce(MenuCommand command)
        {
            if (command.context is AudioSource source) {
                source.Play();
            }
        }
    }
}
#endif