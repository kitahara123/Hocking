using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class MissionManager : MonoBehaviour, IGameManager
    {
        public ManagerStatus status { get; private set; }

        public int curLevel { get; private set; }
        public int maxLevel { get; private set; }

        private string prevScene;

        public void Startup(NetworkService service)
        {
            Debug.Log("Mission manager starting...");

            UpdateData(0, 2);

            SceneManager.sceneLoaded += OnSceneLoaded;
            status = ManagerStatus.Started;
        }

        public void UpdateData(int curLevel, int maxLevel)
        {
            this.curLevel = curLevel;
            this.maxLevel = maxLevel;
        }

        public void ObjectiveReached() => Messenger.Broadcast(GameEvent.LEVEL_COMPLETED);

        private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            if (prevScene == "Managers" || prevScene == null) return;
            SceneManager.UnloadSceneAsync(prevScene);
            SceneManager.SetActiveScene(scene);
        }

        public void GoToNext()
        {
            if (curLevel < maxLevel)
            {
                curLevel++;
                var name = $"Level{curLevel}";
                prevScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);

                Debug.Log($"Unload {prevScene}");
                Debug.Log($"Loading {name}");

            }
            else
            {
                Debug.Log("Last level");
                Messenger.Broadcast(GameEvent.GAME_COMPLETED);
            }
        }

        public void RestartCurrent()
        {
            var name = "Level" + curLevel;
            Debug.Log("Loading " + name);
            prevScene = name;
            SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        }

        private void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}