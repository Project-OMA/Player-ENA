using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ENA.Maps;
using UnityEngine;

namespace ENA.Services
{
    public class LocalCache: MapService.DataSource
    {
        #region Constants
        public const string MapsFolder = "resources/maps/";
        public const string LogsFolder = "logs/";
        #endregion
        #region Properties
        private string MapsFullPath => DataPath.Persistent+MapsFolder;
        private string LogsFullPath => DataPath.Persistent+LogsFolder;
        #endregion
        #region MapService.DataSource Implementation
        public Task<MapData[]> FetchMapsFor(string userToken)
        {
            VerifyMapsFolder();

            var list = new List<MapData>();
            var fullPath = MapsFullPath;
            var directories = Directory.GetDirectories(fullPath);

            Debug.Log($"Load Maps from {fullPath}");

            foreach(var directory in directories) {
                var mapName = directory.Replace(fullPath, string.Empty);
                list.Add(new MapData(mapName, directory+"/"));
            }
            return Task.FromResult(list.ToArray());
        }
        #endregion
        #region Methods
        private void VerifyMapsFolder()
        {
            var fullPath = MapsFullPath;
            if (!Directory.Exists(fullPath)) {
                Directory.CreateDirectory(fullPath);
            }
        }
        #endregion
    }
}