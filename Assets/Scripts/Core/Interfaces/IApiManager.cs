using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Interfaces
{
    public interface IApiManager
    {
        void PostRequest<TData>(string url, Dictionary<string, string> form = null,
            Action<TData> successCallback = null, Action errorCallback = null);

        void PutRequest(string url, byte[] data,
            Action<float> progressCallback = null, Action uploadComplete = null);

        void GetRequest<TData>(string url, Action<TData> successCallback = null);
        
        void PostRequest(string url, Dictionary<string, string> form);
        void GetTexture(string url, Action<Texture> pathCallback);
    }
}