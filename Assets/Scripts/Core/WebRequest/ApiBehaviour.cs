using System;
using System.Collections;
using System.Collections.Generic;
using Core.DependencyManager;
using Core.Interfaces;
using Core.Utils;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using LogType = Core.Utils.Logger.LogType;

namespace Core.WebRequest
{
    public class ApiBehaviour : IApiManager
    {
        private RoutineManager RoutineManager => DI.Get<RoutineManager>();

        private static Texture downloadedTexture;
        
        public void PostRequest<TData>(string url, Dictionary<string, string> form = null,
            Action<TData> successCallback = null, Action errorCallback = null)
        {
            RoutineManager.RoutineStart(PostRequestRoutine(url, form, successCallback));
        }

        public void PostRequest(string url, Dictionary<string, string> form)
        {
            RoutineManager.RoutineStart(PostRequestRoutine(url, form));
        }

        public void GetRequest<TData>(string url, Action<TData> successCallback = null)
        {
            RoutineManager.RoutineStart(GetRequestRoutine(url, successCallback));
        }

        public void GetTexture(string url, Action<Texture> pathCallback)
        {
            RoutineManager.RoutineStart(GetTextureRoutine(url, pathCallback));
        }

        public void PutRequest(string url, byte[] data,
            Action<float> progressCallback = null, Action uploadComplete = null)
        {
            RoutineManager.RoutineStart(PutRequestRoutine(url, data, uploadComplete, progressCallback));
        }

        private IEnumerator PostRequestRoutine<TData>(string url, Dictionary<string, string> form = null,
            Action<TData> successCallback = null, Action errorCallback = null)
        {
            var uwr = UnityWebRequest.Post(url, form);

            yield return uwr.SendWebRequest();

            if (uwr.isHttpError || uwr.isHttpError)
            {
                errorCallback.InvokeSafe();
                
                yield break;
            }

            var requestData = JsonConvert.DeserializeObject<TData>(uwr.downloadHandler.text);

            successCallback.InvokeSafe(requestData);
        }
        
        private IEnumerator PostRequestRoutine(string url, Dictionary<string, string> form)
        {
            var uwr = UnityWebRequest.Post(url, form);
            
            yield return uwr.SendWebRequest();

            if (uwr.isHttpError || uwr.isHttpError)
            {
                this.LogError($"Error \n {url}", LogType.Api);
            }
        }

        private IEnumerator GetRequestRoutine<TData>(string url, Action<TData> successCallback = null)
        {
            var uwr = UnityWebRequest.Get(url);

            yield return uwr.SendWebRequest();

            try
            {
                var requestData = JsonConvert.DeserializeObject<TData>(uwr.downloadHandler.text, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });

                successCallback.InvokeSafe(requestData);
            }
            catch (JsonException jsonException)
            {
                this.LogError(jsonException.Message, LogType.Api);
                
                successCallback.InvokeSafe(default(TData));                
            }
        }
        
        private IEnumerator GetTextureRoutine(string url, Action<Texture> callback)
        {
            using (var uwr = UnityWebRequestTexture.GetTexture(url))
            {
                yield return uwr.SendWebRequest();

                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    callback.InvokeSafe(null);
                    
                    yield break;
                }
                else
                {
                    downloadedTexture = DownloadHandlerTexture.GetContent(uwr);

                    if (downloadedTexture == null)
                    {
                        callback.InvokeSafe(null);
                        
                        yield break;
                    }

                    callback.InvokeSafe(downloadedTexture);
                }
            } 
            
            yield return null;
        }

        private IEnumerator PutRequestRoutine(string url, byte[] data, Action successCallback = null,
            Action<float> progressCallback = null)
        {
            var uwr = UnityWebRequest.Put(url, data);
            
            uwr.uploadHandler = new UploadHandlerRaw(data);
            uwr.SendWebRequest();

            var speed = new List<float>();
            
            while (!uwr.isDone)
            {
                var oldBytes = uwr.uploadedBytes;


                yield return new WaitForEndOfFrame();

                var downloadSpeed = ((uwr.uploadedBytes - oldBytes) / Time.deltaTime) / 1024f / 1024f;

                speed.Add(downloadSpeed);

                progressCallback.InvokeSafe(uwr.uploadProgress);

                if (speed.Count > 0)
                {
                    speed.RemoveAt(0);
                }
            }

            if (uwr.isHttpError || uwr.isNetworkError)
            {
                yield break;
            }

            progressCallback.InvokeSafe(1f);
            successCallback.InvokeSafe();
        }
    }
}