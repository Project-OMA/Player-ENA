using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ENA.Maps;
using UnityEngine;

namespace ENA.Services
{
    public partial class LocalCache: MapService.DataSource
    {
        #region Constants
        public const string MapsFolder = "resources/maps/";
        #endregion
        #region Properties
        private static string MapsFullPath => DataPath.Persistent+MapsFolder;
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
                var mapData = directory.Replace(fullPath, string.Empty).Replace("--", "#").Split('#');
                var mapID = Convert.ToUInt32(mapData[0]);
                var mapName = mapData[1];

                list.Add(new MapData(mapID, mapName, directory+"/"));
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
        #region Static Methods
        public static MapData CreateMap(uint id, string mapName)
        {
            return new MapData(id, mapName, MapsFullPath+id+"--"+mapName+"/");
        }
        #endregion
    }
}