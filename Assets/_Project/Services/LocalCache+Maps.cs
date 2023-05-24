using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ENA.Maps;
using UnityEngine;

namespace ENA.Services
{
    public partial class LocalCache: MapService.IDataSource
    {
        #region Constants
        public const string MapsFolder = "resources/maps/";
        public const string LoadedMapKey = "MapPath";
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

            #if ENABLE_LOG
            Debug.Log($"Load Maps from {fullPath}");
            #endif

            foreach(var directory in directories) {
                var mapData = directory.Replace(fullPath, string.Empty).Replace("--", "#").Split('#');
                var mapID = Convert.ToUInt32(mapData[0]);
                var mapName = mapData[1];

                list.Add(new MapData(mapID, mapName, directory+"/"));
            }

            return Task.FromResult(list.ToArray());
        }
        #endregion
        #region Static Methods
        public static MapData CreateMap(uint id, string mapName)
        {
            return new MapData(id, mapName, MapsFullPath+id+"--"+mapName+"/");
        }

        public static MapData GetLoadedMap()
        {
            throw new System.NotImplementedException();
        }

        public static void SetCurrentMap(MapData map)
        {
            PlayerPrefs.SetString(LoadedMapKey, map.FilePath);
        }

        private static void VerifyMapsFolder()
        {
            var fullPath = MapsFullPath;
            if (!Directory.Exists(fullPath)) {
                Directory.CreateDirectory(fullPath);
            }
        }
        #endregion
    }
}