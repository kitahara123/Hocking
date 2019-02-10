using UnityEngine;

namespace Managers
{
    public class SettingsManager : MonoBehaviour, IGameManager
    {
        [SerializeField] private bool isometric;
        public ManagerStatus status { get; private set; }
        public bool Isometric => isometric;
        public float GlobalSpeed { get; private set; }

        public void Startup(NetworkService service)
        {
            Debug.Log("Settings manager starting...");

            GlobalSpeed = PlayerPrefs.GetFloat("Speed", 1);

            Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
            status = ManagerStatus.Started;
        }

        private void OnSpeedChanged(float value) => GlobalSpeed = value;

        public void IsometricToggle(bool value)
        {
            isometric = value;
            Debug.Log("isometric = "+isometric);
            Messenger<bool>.Broadcast(GameEvent.ISOMETRIC_ENABLED, value, MessengerMode.DONT_REQUIRE_LISTENER);
            if (isometric)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void OnDestroy() => Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }
}