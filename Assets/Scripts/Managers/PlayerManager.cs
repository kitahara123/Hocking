using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class PlayerManager : MonoBehaviour, IGameManager
    {
        public PlayerCharacter Player { get;  set; }
        
        public ManagerStatus status { get; private set; }
        public void Startup(NetworkService service)
        {
            Debug.Log("Player manager starting...");

            status = ManagerStatus.Started;
        }
    }
}