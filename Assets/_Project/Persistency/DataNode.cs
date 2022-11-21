using UnityEngine;

namespace ENA.Persistency
{
    public struct DataNode
    {
        #region Variables
        public uint Version;
        public string Key;
        public object Data;
        #endregion
        #region Methods
        public DataNode(uint version, string key, object data = null)
        {
            Version = version;
            Key = key;
            Data = data;
        }
        #endregion
    }
}