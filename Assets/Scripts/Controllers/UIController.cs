using System.Collections;
using Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreLabel;
        [SerializeField] private TextMeshProUGUI healthLabel;
        [SerializeField] private SettingsPopup settingsPopup;
        [SerializeField] private InventoryPopup inventoryPopup;
        [SerializeField] private TextMeshProUGUI levelEnding;
        [SerializeField] private int scoreDelta;
        [SerializeField] private string levelCompleteMessage = "Level Complete!";
        [SerializeField] private string levelFailedMessage = "Level Failed";
        [SerializeField] private string gameCompleteMessage = "You Finished the Game!";

        private bool scoreLabelOn = true;
        private bool healthLabelOn = true;
        private bool inventoryOn = true;

        private int score;

        public void Awake()
        {
            Messenger.AddListener(GameEvent.ENEMY_HIT, OnEnemyHit);
            Messenger.AddListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
            Messenger.AddListener(GameEvent.LEVEL_COMPLETED, OnLevelComplete);
            Messenger.AddListener(GameEvent.LEVEL_FAILED, OnLevelFailed);
            Messenger.AddListener(GameEvent.GAME_COMPLETED, OnGameComplete);

            if (!Managers.Managers.Settings.Isometric)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            RefreshUI();
            OnHealthUpdated();
            OnScoreChanged(0);
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
            if (healthLabel == null) healthLabelOn = false;
            else healthLabel.gameObject.SetActive(true);

            if (scoreLabel == null) scoreLabelOn = false;
            else scoreLabel.gameObject.SetActive(true);

            if (inventoryPopup == null) inventoryOn = false;
        }

        private void OnEnemyHit() => OnScoreChanged(scoreDelta);

        private void OnScoreChanged(int value)
        {
            if (!scoreLabelOn) return;
            score += value;
            scoreLabel.text = score.ToString();
        }

        private void OnHealthUpdated()
        {
            if (!healthLabelOn) return;
            healthLabel.text = $"Health: {Managers.Managers.Player.health} / {Managers.Managers.Player.maxHealth}";
        }

        private void Update()
        {
            if (Input.GetButtonDown("Cancel")) OnOpenSettings();

            if (Input.GetButtonDown("Inventory") && inventoryOn)
            {
                if (settingsPopup.isActiveAndEnabled) settingsPopup.Close();
                inventoryPopup.OpenClose();
            }
        }

        public void OnOpenSettings()
        {
            if (inventoryOn && inventoryPopup.isActiveAndEnabled) inventoryPopup.Close();
            settingsPopup.OpenClose();
        }

        private void OnLevelComplete() => StartCoroutine(CompleteLevel());

        private IEnumerator CompleteLevel()
        {
            levelEnding.gameObject.SetActive(true);
            levelEnding.text = levelCompleteMessage;
            yield return new WaitForSeconds(2);

            Managers.Managers.Mission.GoToNext();
        }

        private void OnLevelFailed() => StartCoroutine(FailLevel());

        private IEnumerator FailLevel()
        {
            levelEnding.gameObject.SetActive(true);
            levelEnding.text = levelFailedMessage;
            yield return new WaitForSeconds(2);

            Managers.Managers.Player.Respawn();
            Managers.Managers.Mission.RestartCurrent();
        }

        private void OnGameComplete()
        {
            levelEnding.gameObject.SetActive(true);
            levelEnding.text = gameCompleteMessage;
        }

        public void SaveGame() => Managers.Managers.Data.SaveGameState();
        public void LoadGame() => Managers.Managers.Data.LoadGameState();
    }
}