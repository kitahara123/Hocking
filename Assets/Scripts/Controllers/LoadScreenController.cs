using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controllers
{
    public class LoadScreenController : MonoBehaviour
    {
        [SerializeField] private Image progressBar;

        private void Awake()
        {
            Messenger<float>.AddListener(SystemEvent.LOADING_PROGRESS, SetProgress);
            Messenger.AddListener(SystemEvent.MANAGERS_STARTED, OnManagersStarted);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            Messenger<float>.RemoveListener(SystemEvent.LOADING_PROGRESS, SetProgress);
            Messenger.RemoveListener(SystemEvent.MANAGERS_STARTED, OnManagersStarted);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnManagersStarted()
        {
            Managers.Managers.Mission.GoToNext();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            if (scene.name == "Managers") return;
            gameObject.SetActive(false);
        }

        private void SetProgress(float value)
        {
            progressBar.fillAmount = value;
        }
    }
}