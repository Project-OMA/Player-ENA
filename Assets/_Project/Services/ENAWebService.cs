using System.Threading.Tasks;
using ENA.Maps;
using UnityEngine;

namespace ENA.Services
{
    public class ENAWebService: MapService.DataSource, AuthService.CredentialManager
    {
        #region Constants
        public const string ENAServicePath = "https://localhost:7073/api";
        #endregion
        #region Classes
        public struct URI {
            public const string Maps = "/maps";
            public const string User = "/user";
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
            //const string mapURL = ENAServicePath+URI.Maps;
            var response = await apiService.MakeRequest(URI.Maps);
            Debug.Log($"Response: {response}");
            return new MapData[0];
            
            // string PathToMaps = DataPath.Default+"Assets/_Placeholder/Maps/";
            // var maps = new MapData[3]{
            //     new MapData("Casa Simples", PathToMaps+"Casa Simples/"),
            //     new MapData("Escola", PathToMaps+"Escola/"),
            //     new MapData("Escritório", PathToMaps+"Escritório/")
            // };
            // return maps;
        }
        #endregion
        #region AuthService.CredentialManager
        public Task<ENAProfile> ValidateCredentials(string login, string password)
        {
            string profileName = login;
            var loggedProfile = new ENAProfile(profileName);
            return Task.FromResult(loggedProfile);
        }

        public Task<ENAProfile> ValidateToken(string userToken)
        {
            throw new System.NotImplementedException();
        }
        #endregion
        #region Debug
        #if UNITY_EDITOR
        [ContextMenu("Try HTTP GET")]
        public async void TryRequestGet()
        {
            var testAPI = "https://api.quotable.io";
            var testRequest = "/random";

            var quoteData = await WebService.Request(testAPI+testRequest);
            Debug.Log(quoteData);
        }
        #endif
        #endregion
    }
}