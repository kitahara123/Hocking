using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    /// <summary>
    /// Менеджер загрузки картинок из интернета (например рекламы)
    /// </summary>
    public class ImagesManager : MonoBehaviour, IGameManager
    {
        public ManagerStatus status { get; private set; }
        private NetworkService network;
        private string[] urls =
        {
            "https://pp.userapi.com/c850124/v850124341/11a24/7ngytSS7M3o.jpg",
            "https://pp.userapi.com/c846417/v846417686/e9fa1/rpMpnHzrc_c.jpg",
            "https://pp.userapi.com/c849528/v849528587/4d44a/2X-u6mjXyhM.jpg",
            "https://pp.userapi.com/c846419/v846419092/154ee/rBZ8YFG9H4M.jpg",
            "https://sun1-18.userapi.com/c7004/v7004120/3eff1/CsrDOUC6F3c.jpg"
        };
        
        public void Startup(NetworkService service)
        {
            Debug.Log("Image manager starting...");
            network = service;
            status = ManagerStatus.Started;
        }

        public void GetWebImage(Action<Texture2D> callback)
        {
                StartCoroutine(network.DownloadImage(urls[Random.Range(0, urls.Length)], callback));
        }
    }
}