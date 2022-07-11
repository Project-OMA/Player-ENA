using System.Collections.Generic;
using System.IO;
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
        #endregion
        #region Constructors
        public MapService() {}
        #endregion
        #region Methods
        public Task<MapData[]> FetchMaps(ENAProfile profile)
        {
            return dataSource.FetchMapsFor(profile.UserID);
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
        public static MapData[] LoadMaps(string jsonFile)
        {
            if (!File.Exists(jsonFile)) return new MapData[0];
            string jsonString = File.ReadAllText(jsonFile);

            var obj = JsonUtility.FromJson<MapData.List>(jsonFile);
            return obj.maps;
        }

        public static bool SaveMaps(MapData[] maps)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}