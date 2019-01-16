using UnityEngine;

namespace Managers
{
    public class SettingsManager : MonoBehaviour, IGameManager
    {
        [SerializeField] private bool isometric;
        public ManagerStatus status { get; private set; }
        public bool Isometric => isometric;
        
        public void Startup(NetworkService service)
        {
            Debug.Log("Settings manager starting...");

            status = ManagerStatus.Started;
        }

        public void IsometricToggle(bool value) => isometric = value;
    }
}