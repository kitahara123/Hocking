using System.Collections;
using Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class MissionManager : MonoBehaviour, IGameManager
    {

        [SerializeField] public string[] levelSequence;
        
        private const string MANAGERS_SCENE = "Managers";
        public ManagerStatus status { get; private set; }

        public int curLevel { get; private set; }
        public int maxLevel { get; private set; }
        

        private string prevScene;

        public void Startup(NetworkService service)
        {
            Debug.Log("Mission manager starting...");

            UpdateData(-1, levelSequence.Length -1);

            Messenger.AddListener(SystemEvent.MANAGERS_STARTED, GoToNext);
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
            if (prevScene != MANAGERS_SCENE && prevScene != null)
            {
                SceneManager.UnloadSceneAsync(prevScene);
                Debug.Log($"Unload {prevScene} scene");
            }
            SceneManager.SetActiveScene(scene);
        }

        public void GoToNext()
        {
            if (curLevel < maxLevel)
            {
                Managers.UI.LoadScreen.gameObject.SetActive(true);

                curLevel++;
                var name = levelSequence[curLevel];
                prevScene = SceneManager.GetActiveScene().name;
                StartCoroutine(LoadScene(name));

                Debug.Log($"Loading {name} scene");
            }
            else
            {
                Debug.Log("Last level");
                Messenger.Broadcast(GameEvent.GAME_COMPLETED);
            }
        }

        private IEnumerator LoadScene(string sceneName)
        {
            var load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!load.isDone)
            {
                var progress = load.progress;
                if (prevScene == MANAGERS_SCENE) progress = load.progress / 2 + 0.5f;
                
                Messenger<float>.Broadcast(SystemEvent.LOADING_PROGRESS, progress);
                yield return null;
            }
        }

        public void RestartCurrent()
        {
            Managers.UI.LoadScreen.gameObject.SetActive(true);
            var name = levelSequence[curLevel];
            Debug.Log("Loading " + name);
            prevScene = name;
            StartCoroutine(LoadScene(name));
        }

        private void OnDestroy()
        {
            Messenger.RemoveListener(SystemEvent.MANAGERS_STARTED, GoToNext);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}