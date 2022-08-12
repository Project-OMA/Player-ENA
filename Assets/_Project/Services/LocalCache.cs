using System.Collections.Generic;
using ENA.Maps;
using UnityEngine;

namespace ENA.Services
{
    public partial class LocalCache
    {
        #region Constants
        public const string LogsFolder = "logs/";
        #endregion
        #region Properties
        private string LogsFullPath => DataPath.Persistent+LogsFolder;
        #endregion
    }
}