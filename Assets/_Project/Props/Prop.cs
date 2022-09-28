using System;
using UnityEngine;

namespace ENA.Props
{
    [CreateAssetMenu(fileName="New Prop", menuName="ENA/Prop Model")]
    public class Prop: ScriptableObject
    {
        #region Class
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
        [SerializeField] public GameObject Prefab;
        #endregion
    }
}