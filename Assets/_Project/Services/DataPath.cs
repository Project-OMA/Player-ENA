using UnityEngine;

namespace ENA.Services
{
    public class DataPath
    {
        #region Static Properties
        public static string Default => GameDataFolder();
        public static string Persistent => PersistentFolder();
        #endregion
        #region Static Methods
        private static string GameDataFolder()
        {
            #if UNITY_EDITOR
            string dataPath = Application.dataPath;
            return dataPath.Substring(0, Mathf.Max(dataPath.Length - 7, 0)) + "/";
            #elif UNITY_STANDALONE_OSX
            return Application.dataPath + "/Resources/Data/";
            #else
            return Application.dataPath + "/";
            #endif
        }

        private static string PersistentFolder()
        {
            return Application.persistentDataPath + "/";
        }
        #endregion
    }
}