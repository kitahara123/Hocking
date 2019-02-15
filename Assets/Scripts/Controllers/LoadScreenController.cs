using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controllers
{
    /// <summary>
    /// Заглушка на загрузке уровней
    /// </summary>
    public class LoadScreenController : MonoBehaviour
    {
        [SerializeField] private Image progressBar;

        private void Awake()
        {
            Messenger<float>.AddListener(SystemEvent.LOADING_PROGRESS, SetProgress);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            Messenger<float>.RemoveListener(SystemEvent.LOADING_PROGRESS, SetProgress);
            SceneManager.sceneLoaded -= OnSceneLoaded;
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