using System;
using UnityEngine;

namespace ENA.Maps
{
    [CreateAssetMenu(fileName="New Group", menuName="ENA/Prop Group")]
    public class PropGroup: ScriptableObject, IPropDataSource
    {
        #region Classes
        [Serializable]
        public struct Entry
        {
            #region Variables
            [SerializeField] public Prop Prop;
            [SerializeField] public Prop.Preset[] Presets;
            #endregion
            #region Methods
            public bool GetPreset(string code, out Prop.Preset preset)
            {
                foreach(var item in Presets) {
                    if (item.Code == code) {
                        preset = item;
                        return true;
                    }
                }

                preset = default;
                return false;
            }
            #endregion
        }
        #endregion
        #region Variables
        [SerializeField] Entry[] entries;
        #endregion
        #region IPropDataSource Implementation
        public bool FetchProp(string inputCode, out Prop.Spawn spawn)
        {
            foreach(var entry in entries) {
                if (entry.GetPreset(inputCode, out var preset)) {
                    spawn = new Prop.Spawn(entry.Prop, preset);
                    return true;
                }
            }

            spawn = default;
            return false;
        }
        #endregion
    }
}