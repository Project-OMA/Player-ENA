using ENA.Maps;
using UnityEngine;
using UnityEngine.Localization;

namespace ENA.Maps
{
    public class CollidableProp: MonoBehaviour
    {
        #region Variables
        [field: SerializeField] public Prop Prop {get; private set;}
        [field: SerializeField] public AudioSource CollisionAudioSource {get; private set;}
        #endregion
    }
}