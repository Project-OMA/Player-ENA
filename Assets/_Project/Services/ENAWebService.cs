using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ENA.Maps;
using ENA.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace ENA.Services
{
    public class ENAWebService: MapService.IDataSource, AuthService.CredentialManager
    {
        #region Constants
        public const string ENAServicePath = "https://localhost:7073/api";
        #endregion
        #region Classes
        public struct URI {
            public const string Maps = "maps";
            public const string User = "user";
        }
        [System.Serializable]
        public class MapPayload {
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
            string profileName = login;
            var loggedProfile = new ENAProfile(profileName, profileName.GetHashCode());
            return Task.FromResult(loggedProfile);
        }

        public Task<ENAProfile> ValidateToken(string userToken)
        {
            throw new System.NotImplementedException();
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
            var mapRequest = apiService.Get(URI.Maps);
            MapPayload[] payload = new MapPayload[0];

            using (var response = await apiService.Get(URI.Maps).Send()) {
                try {
                    payload = Decode(response.Handler.text);
                } catch(Exception e) {
                    #if ENABLE_LOG
                    Debug.LogWarning("JSON parsing has failed!\n"+e.StackTrace);
                    #endif
                }
            }

            return payload;
        }

        private async Task Validate(MapData map, MapPayload payload)
        {
            await Download(payload.mapURL, map.FilePath);
            await DownloadImage(payload.imageURL, map.ThumbnailPath);
        }
        #endregion
        #region Debug
        #if UNITY_EDITOR
        [MenuItem("ENA/Services/Connection Test")]
        public static async void TryRequestGet()
        {
            const string testAPI = "https://api.quotable.io";
            const string testRequest = "/random";

            using var response = await WebService.HTTPGet(testAPI + testRequest).Send();
            Debug.Log(response.Handler.text);
        }
        #endif
        #endregion
    }
}