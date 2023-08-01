using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ENA.Maps;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace ENA.Services
{
    public class ENAWebService: MapService.IDataSource, AuthService.ICredentialManager
    {
        #region Constants
        public const string ENAServicePath = "https://localhost:7073/api";
        #endregion
        #region Classes
        public struct URI
        {
            public const string Maps = "maps";
            public const string User = "user";
        }

        [Serializable]
        public class MapPayload
        {
            public uint id;
            public string mapName;
            public string mapURL;
            public string imageURL;
        }
        #endregion
        #region Variables
        WebService apiService;
        #endregion
        #region Constructors
        public ENAWebService()
        {
            apiService = new WebService(ENAServicePath);
        }
        #endregion
        #region MapService.DataSource Implementation
        public async Task<MapData[]> FetchMapsFor(string userID)
        {
            var maps = new List<MapData>();
            var payload = await FetchPayload();

            foreach(var data in payload) {
                var map = LocalCache.CreateMap(data.id, data.mapName);
                await Validate(map, data);
                maps.Add(map);
            }

            return maps.ToArray();
        }
        #endregion
        #region AuthService.CredentialManager
        public Task<ENAProfile> ValidateCredentials(string login, string password)
        {
            return Task.FromResult(new ENAProfile(login, login.GetHashCode()));
        }

        public Task<ENAProfile> ValidateToken(string userToken)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Methods
        private MapPayload[] Decode(string jsonString)
        {
            return Json.DecodeArray<MapPayload>(jsonString, false);
        }

        private async Task<bool> Download(string url, string destination)
        {
            using var response = await apiService.Get(url).Send();
            var handler = response.Handler;

            if (string.IsNullOrEmpty(handler.text)) {
                return false;
            } else {
                await handler.WriteToFile(destination);
                return true;
            }
        }

        private async Task<bool> DownloadImage(string url, string destination)
        {
            using var response = await apiService.Image(url).Send();
            var imageHandler = response.Handler.ToImage();

            if (imageHandler?.texture == null) {
                return false;
            } else {
                await imageHandler.WriteToFile(destination);
                return true;
            }
        }

        public async Task<MapPayload[]> FetchPayload()
        {
            using var response = await apiService.Get(URI.Maps).Send();
            try {
                return Decode(response.Handler.text);
            } catch {
                #if ENABLE_LOG
                Debug.LogWarning("JSON parsing has failed!\n"+e.StackTrace);
                #endif
                return new MapPayload[0];
            }
        }

        private async Task Validate(MapData map, MapPayload payload)
        {
            await Download(payload.mapURL, map.FilePath);
            await DownloadImage(payload.imageURL, map.ThumbnailPath);
        }
        #endregion
    }
}