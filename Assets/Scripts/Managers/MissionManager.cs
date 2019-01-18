using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class MissionManager : MonoBehaviour, IGameManager
    {
        public ManagerStatus status { get; private set; }
        
        public int curLevel { get; private set; }
        public int maxLevel { get; private set; }
        
        public void Startup(NetworkService service)
        {
            Debug.Log("Mission manager starting...");

            curLevel = 0;
            maxLevel = 1;

            status = ManagerStatus.Started;
        }

        public void GoToNext()
        {
            if (curLevel < maxLevel)
            {
                curLevel++;
                var name = $"Level {curLevel}";
                Debug.Log($"Loading {name}");
                SceneManager.LoadScene(name);
            }
            else
            {
                Debug.Log("Last level");
            }
        }
    }
}