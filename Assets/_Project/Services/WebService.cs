using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

namespace ENA.Services
{
    public struct WebService
    {
        #region Constants
        public const string DefaultContentType = "application/json";
        #endregion
        #region Classes
        public struct Request: IDisposable
        {
            #region Variables
            UnityWebRequest request;
            #endregion
            #region Constructors
            public Request(UnityWebRequest request)
            {
                this.request = request;
            }

            public Request(UnityWebRequest request, string contentType)
            {
                this.request = request;
                request.SetRequestHeader("Content-Type", contentType);
            }
            #endregion
            #region IDisposable Implementation
            public void Dispose()
            {
                request?.Dispose();
            }
            #endregion
            #region Methods
            public void Cancel()
            {
                request?.Abort();
            }

            public async Task<Response> Send()
            {
                Response response;

                using (request) {
                    request.disposeDownloadHandlerOnDispose = false;
                    var operation = request.SendWebRequest();

                    do await Task.Delay(10);
                    while(!operation.isDone);

                    if (request.result != UnityWebRequest.Result.Success) {
                        #if ENABLE_LOG
                        Debug.LogWarning(request.error);
                        #endif
                    }

                    response = new Response(request);
                }

                return response;
            }
            #endregion
        }

        public struct Response: IDisposable
        {
            #region Variables
            public DownloadHandler Handler {get; private set;}
            public long ResponseCode {get; private set;}
            public UnityWebRequest.Result Result {get; private set;}
            #endregion
            #region Constructors
            public Response(UnityWebRequest request)
            {
                Handler = request.downloadHandler;
                ResponseCode = request.responseCode;
                Result = request.result;
            }
            #endregion
            #region IDisposable Implementation
            public void Dispose()
            {
                Handler?.Dispose();
            }
            #endregion
        }
        #endregion
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
        public Request Audio(string subpath, AudioType type = AudioType.AUDIOQUEUE)
        {
            return WebService.AudioRequest(Path.Combine(apiRoot,subpath));
        }

        public Request Delete(string subpath)
        {
            return WebService.HTTPDelete(Path.Combine(apiRoot,subpath));
        }

        public Request Get(string subpath, string contentType = DefaultContentType)
        {
            return WebService.HTTPGet(Path.Combine(apiRoot,subpath), contentType);
        }

        public Request Post(string subpath, string requestData, string contentType = DefaultContentType)
        {
            return WebService.HTTPPost(Path.Combine(apiRoot,subpath), requestData, contentType);
        }

        public Request Put(string subpath, string requestData)
        {
            return WebService.HTTPPut(Path.Combine(apiRoot,subpath), requestData);
        }

        public Request Image(string subpath)
        {
            return WebService.ImageRequest(Path.Combine(apiRoot,subpath));
        }
        #endregion
        #region Static Methods
        private static Request AudioRequest(string path, AudioType type = AudioType.AUDIOQUEUE)
        {
            return new Request(UnityWebRequestMultimedia.GetAudioClip(path, type));
        }

        public static Request HTTPDelete(string path)
        {
            return new Request(WebRequest(path, RequestType.DELETE));
        }

        public static Request HTTPGet(string path, string contentType = DefaultContentType)
        {
            return new Request(WebRequest(path, RequestType.GET), contentType);
        }

        public static Request HTTPPost(string path, string requestData, string contentType = DefaultContentType)
        {
            return new Request(WebRequest(path, RequestType.POST, requestData), contentType);
        }

        public static Request HTTPPut(string path, string requestData)
        {
            return new Request(WebRequest(path, RequestType.PUT, requestData));
        }

        private static Request ImageRequest(string path)
        {
            return new Request(UnityWebRequestTexture.GetTexture(path));
        }

        private static UnityWebRequest WebRequest(string path, RequestType requestType = RequestType.GET, string requestData = null)
        {
            switch (requestType) {
                case RequestType.GET:
                    return UnityWebRequest.Get(path);
                case RequestType.POST:
                    return UnityWebRequest.PostWwwForm(path, requestData);
                case RequestType.PUT:
                    return UnityWebRequest.Put(path, requestData);
                case RequestType.DELETE:
                    return UnityWebRequest.Delete(path);
                default:
                    return default(UnityWebRequest);
            }
        }
        #endregion
    }
}