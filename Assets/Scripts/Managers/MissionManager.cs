using System.Collections;
using Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class MissionManager : MonoBehaviour, IGameManager
    {
        [SerializeField] private string[] levelSequence;
        [SerializeField] private LoadScreenController LoadScreen;

        private const string MANAGERS_SCENE = "Managers";
        public ManagerStatus status { get; private set; }

        public int curLevel { get; private set; }
        public string prevScene { get; private set; }

        public bool SceneLoaded { get; private set; }

        public void Startup(NetworkService service)
        {
            Debug.Log("Mission manager starting...");

            UpdateData(-1);

            Messenger.AddListener(SystemEvent.MANAGERS_STARTED, GoToNext);
            SceneManager.sceneLoaded += OnSceneLoaded;
            status = ManagerStatus.Started;
        }

        public void UpdateData(int curLevel)
        {
            this.curLevel = curLevel;
        }

        public void ObjectiveReached() => Messenger.Broadcast(GameEvent.LEVEL_COMPLETED);

        private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            Debug.Log("Scene loaded: " + scene.name);
            SceneLoaded = true;
            
            SceneManager.SetActiveScene(scene);
        }

        public void GoToNext()
        {
            if (curLevel < levelSequence.Length)
            {
                LoadScreen.gameObject.SetActive(true);

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
            SceneLoaded = false;
            
            if (prevScene != MANAGERS_SCENE && prevScene != null && SceneManager.GetSceneByName(prevScene).isLoaded)
            {
                Debug.Log($"Unload: {prevScene} scene");
                SceneManager.UnloadSceneAsync(prevScene);
            }
            
            var load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            Debug.Log("Loading: " + sceneName);
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
            LoadScreen.gameObject.SetActive(true);
            var name = levelSequence[curLevel];
            prevScene = name;
            StartCoroutine(LoadScene(name));
        }

        public void RestartGame()
        {
            LoadScreen.gameObject.SetActive(true);
            prevScene = SceneManager.GetActiveScene().name;
            curLevel = 0;
            var name = levelSequence[curLevel];
            StartCoroutine(LoadScene(name));
        }

        private void OnDestroy()
        {
            Messenger.RemoveListener(SystemEvent.MANAGERS_STARTED, GoToNext);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}