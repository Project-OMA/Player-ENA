using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ENA.Maps;
using UnityEngine;

namespace ENA.Services
{
    public class MapService: IService
    {
        #region Interfaces
        public interface DataSource
        {
            public Task<MapData[]> FetchMapsFor(string userToken);
        }
        #endregion
        #region Variables
        DataSource dataSource;
        LocalCache localCache;
        #endregion
        #region Constructors
        public MapService()
        {
            localCache = new LocalCache();
        }
        #endregion
        #region Methods
        public async Task<MapData[]> FetchMaps(ENAProfile profile)
        {
            var userToken = $"{profile.UserID}";
            var results = await Task.WhenAll(localCache.FetchMapsFor(userToken), dataSource.FetchMapsFor(userToken));
            var mapList = results.SelectMany(result => result).Distinct(MapData.IDComparer.New).ToList();
            ValidateMaps(mapList);
            return mapList.ToArray();
        }

        private void ValidateMaps(List<MapData> mapList)
        {
            for(int i = mapList.Count-1; i > 0; i--) {
                var map = mapList[i];
                Debug.Log($"Map ID: {map.ID}");
                if (!File.Exists(map.FilePath)) {
                    mapList.RemoveAt(i);
                }
            }
        }

        public void SetDataSource(DataSource source)
        {
            dataSource = source;
        }
        
        public Task<bool> UploadMap()
        {
            throw new System.NotImplementedException();
        }
        #endregion
        #region Static Methods
        public static bool SaveMaps(MapData[] maps, string targetFile)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}