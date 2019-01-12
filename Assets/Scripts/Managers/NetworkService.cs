using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class NetworkService
    {
        private Dictionary<string, Texture2D> cache;

        public NetworkService()
        {
            cache = new Dictionary<string, Texture2D>();
        }

        public IEnumerator DownloadImage(string url, Action<Texture2D> callback)
        {
            if (cache.ContainsKey(url))
            {
                Debug.Log("cached");
                callback(cache[url]);
            }
            else
            {
                var www = new WWW(url);
                yield return www;
                cache.Add(url, www.texture);
                callback(www.texture);
            }
        }
    }
}