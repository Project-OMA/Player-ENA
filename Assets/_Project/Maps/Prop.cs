using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace ENA.Maps
{
    [CreateAssetMenu(fileName="New Prop", menuName="ENA/Prop Model")]
    public class Prop: ScriptableObject
    {
        #region Classes
        [Serializable]
        public struct Preset
        {
            #region Variables
            [SerializeField] public string Code;
            [Header("Parameters")]
            [SerializeField] public Vector3 Rotation;
            #endregion
        }

        [Serializable]
        public class Spawn
        {
            #region Variables
            [SerializeField] public GameObject Prefab;
            [SerializeField] public Vector3 Rotation;
            #endregion
            #region Constructor
            public Spawn(Prop prop, Preset preset)
            {
                Prefab = prop.Prefab;
                Rotation = preset.Rotation;
            }
            #endregion
        }
        #endregion
        #region Variables
        [SerializeField] string id;
        [SerializeField] LocalizedString propName;
        [SerializeField] GameObject prefab;
        #endregion
        #region Properties
        public string ID => id;
        public GameObject Prefab => prefab;
        public string Name => propName.GetLocalizedString();
        #endregion
    }
}