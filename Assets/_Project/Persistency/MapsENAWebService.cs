using ENA.Utilities.Services;
using UnityEngine;

namespace ENA.Persistency
{
    public class MapsENAWebService: MonoBehaviour
    {
        #region Constants
        public const string ENAServicePath = "http://localhost:3000";
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
        #region Methods
        void Start()
        {

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