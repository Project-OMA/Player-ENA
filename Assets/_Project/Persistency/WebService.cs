using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

namespace JurassicEngine.Services
{
    public struct WebService
    {
        #region Enums
        public enum RequestType {
            GET, POST, PUT, DELETE
        }
        #endregion
        #region Variables
        string apiRoot;
        #endregion
        #region Constructors
        public WebService(string serviceAddress)
        {
            apiRoot = serviceAddress;
        }
        #endregion
        #region Methods
        public async Task<string> MakeRequest(string subpath, RequestType requestType = RequestType.GET, string requestData = null)
        {
            return await WebService.Request(apiRoot+subpath, requestType, requestData);
        }

        public async Task<T> MakeRequest<T>(string subpath, RequestType requestType = RequestType.GET, string requestData = null)
        {
            return await WebService.Request<T>(apiRoot+subpath, requestType, requestData);
        }

        public async Task<AudioClip> MakeRequestAudio(string path, AudioType type = AudioType.AUDIOQUEUE)
        {
            return await WebService.RequestAudio(path, type);
        }

        public async Task<Texture2D> MakeRequestTexture(string subpath, RequestType requestType = RequestType.GET, string requestData = null)
        {
            return await WebService.RequestTexture(apiRoot+subpath);
        }
        #endregion
        #region Static Methods
        public static UnityWebRequest CreateAudioRequest(string path, AudioType type = AudioType.AUDIOQUEUE)
        {
            return UnityWebRequestMultimedia.GetAudioClip(path,type);
        }

        public static UnityWebRequest CreateImageRequest(string path)
        {
            return UnityWebRequestTexture.GetTexture(path);
        }

        public static UnityWebRequest CreateWebRequest(string path, RequestType requestType = RequestType.GET, string requestData = null)
        {
            switch (requestType) {
                case RequestType.GET:
                    return UnityWebRequest.Get(path);
                case RequestType.POST:
                    return UnityWebRequest.Post(path, requestData);
                case RequestType.PUT:
                    return UnityWebRequest.Put(path, requestData);
                case RequestType.DELETE:
                    return UnityWebRequest.Delete(path);
                default:
                    return default(UnityWebRequest);
            }
        }

        public static async Task<string> Request(string path, RequestType requestType = RequestType.GET, string requestData = null)
        {
            var apiRequest = CreateWebRequest(path);
            apiRequest.SetRequestHeader("Content-Type", "application/json");
            var operation = apiRequest.SendWebRequest();

            while(!operation.isDone) await Task.Yield();

            if (apiRequest.result != UnityWebRequest.Result.Success) {
                Debug.LogError(apiRequest.error);
            } else {
                return apiRequest.downloadHandler.text;
            }

            return null;
        }

        public static async Task<T> Request<T>(string path, RequestType requestType = RequestType.GET, string requestData = null)
        {
            var result = await WebService.Request(path, requestType, requestData);
            return JsonUtility.FromJson<T>(result);
        }

        public static async Task<AudioClip> RequestAudio(string path, AudioType type = AudioType.AUDIOQUEUE)
        {
            var audioRequest = CreateAudioRequest(path, type);
            var operation = audioRequest.SendWebRequest();

            while(!operation.isDone) await Task.Yield();

            if (audioRequest.result != UnityWebRequest.Result.Success) {
                Debug.LogError(audioRequest.error);
            } else {
                return DownloadHandlerAudioClip.GetContent(audioRequest);
            }

            return null;
        }

        public static async Task<Texture2D> RequestTexture(string path)
        {
            var imageRequest = CreateImageRequest(path);
            var operation = imageRequest.SendWebRequest();

            while(!operation.isDone) await Task.Yield();

            if (imageRequest.result != UnityWebRequest.Result.Success) {
                Debug.LogError(imageRequest.error);
            } else {
                return DownloadHandlerTexture.GetContent(imageRequest);
            }

            return null;
        }
        #endregion
    }
}