using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class StartupController : MonoBehaviour
    {
        [SerializeField] private  Image progressBar;

        private void Awake()
        {
            Messenger<int, int>.AddListener(StartupEvent.MANAGERS_PROGRESS, OnManagersProgress);
            Messenger.AddListener(StartupEvent.MANAGERS_STARTED, OnManagersStarted);
        }

        private void OnDestroy()
        {
            Messenger<int, int>.RemoveListener(StartupEvent.MANAGERS_PROGRESS, OnManagersProgress);
            Messenger.RemoveListener(StartupEvent.MANAGERS_STARTED, OnManagersStarted);
        }

        private void OnManagersStarted()
        {
            Managers.Managers.Mission.GoToNext();
        }

        private void OnManagersProgress(int nunReady, int numModules)
        {
            var progress = (float) nunReady / numModules;
            progressBar.fillAmount = progress;
        }
    }
}