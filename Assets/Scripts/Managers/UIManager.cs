using System.Collections;
using Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class UIManager : MonoBehaviour, IGameManager
    {
        [SerializeField] private TextMeshProUGUI scoreLabel;
        public bool scoreLabelOn;
        [SerializeField] private TextMeshProUGUI healthLabel;
        public bool healthLabelOn;
        [SerializeField] private SettingsPopup settingsPopup;
        public bool settingsOn;
        [SerializeField] private InventoryPopup inventoryPopup;
        public bool inventoryOn;
        [SerializeField] private int scoreDelta;
        [SerializeField] private TextMeshProUGUI levelEnding;
        [SerializeField] private string levelCompleteMessage = "Level Complete!";
        [SerializeField] private string levelFailedMessage = "Level Failed";
        [SerializeField] private string gameCompleteMessage = "You Finished the Game!";
        [SerializeField] private LoadScreenController loadScreen;
        public LoadScreenController LoadScreen => loadScreen;

        private int score;

        public ManagerStatus status { get; private set; }

        public void Startup(NetworkService service)
        {
            Debug.Log("UI manager starting...");
            
            Messenger.AddListener(GameEvent.ENEMY_HIT, OnEnemyHit);
            Messenger.AddListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
            Messenger.AddListener(GameEvent.LEVEL_COMPLETED, OnLevelComplete);
            Messenger.AddListener(GameEvent.LEVEL_FAILED, OnLevelFailed);
            Messenger.AddListener(GameEvent.GAME_COMPLETED, OnGameComplete);
            
            if (!Managers.Settings.Isometric)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            
            OnHealthUpdated();
            score = 0;
            scoreLabel.text = score.ToString();
            RefreshUI();
            
            status = ManagerStatus.Started;
        }

        private void OnDestroy()
        {
            Messenger.RemoveListener(GameEvent.ENEMY_HIT, OnEnemyHit);
            Messenger.RemoveListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
            Messenger.RemoveListener(GameEvent.LEVEL_COMPLETED, OnLevelComplete);
            Messenger.RemoveListener(GameEvent.LEVEL_FAILED, OnLevelFailed);
            Messenger.RemoveListener(GameEvent.GAME_COMPLETED, OnGameComplete);
        }

        public void RefreshUI()
        {
            scoreLabel.gameObject.SetActive(scoreLabelOn);
            healthLabel.gameObject.SetActive(healthLabelOn);
        }

        public void OnOpenSettings() => settingsPopup.OpenClose();

        private void OnEnemyHit()
        {
            score += scoreDelta;
            scoreLabel.text = score.ToString();
        }

        private void OnHealthUpdated()
        {
            healthLabel.text = $"Health: {Managers.Player.health} / {Managers.Player.maxHealth}";
        }

        private void Update()
        {
            if (Input.GetButtonDown("Cancel") && settingsOn)
            {
                if (inventoryPopup.isActiveAndEnabled) inventoryPopup.Close();
                settingsPopup.OpenClose();
            }

            if (Input.GetButtonDown("Inventory") && inventoryOn)
            {
                if (settingsPopup.isActiveAndEnabled) settingsPopup.Close();
                inventoryPopup.OpenClose();
            }
        }

        private void OnLevelComplete() => StartCoroutine(CompleteLevel());

        private IEnumerator CompleteLevel()
        {
            levelEnding.gameObject.SetActive(true);
            levelEnding.text = levelCompleteMessage;
            yield return new WaitForSeconds(2);

            Managers.Mission.GoToNext();
        }

        private void OnLevelFailed() => StartCoroutine(FailLevel());

        private IEnumerator FailLevel()
        {
            levelEnding.gameObject.SetActive(true);
            levelEnding.text = levelFailedMessage;
            yield return new WaitForSeconds(2);

            Managers.Player.Respawn();
            Managers.Mission.RestartCurrent();
        }

        private void OnGameComplete()
        {
            levelEnding.gameObject.SetActive(true);
            levelEnding.text = gameCompleteMessage;
        }

        public void SaveGame() => Managers.Data.SaveGameState();
        public void LoadGame() => Managers.Data.LoadGameState();
    }
}