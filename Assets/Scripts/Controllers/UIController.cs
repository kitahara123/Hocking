using System.Collections;
using Managers;
using TMPro;
using UnityEngine;

namespace Controllers
{
    /// <summary>
    /// Класс контролирующий интерфейс
    /// </summary>
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
        [SerializeField] private int scoresToWin = 40;

        private bool scoreLabelOn = true;
        private bool healthLabelOn = true;
        private bool inventoryOn = true;

        private int score;

        public void Awake()
        {
            Messenger.AddListener(GameEvent.SCORE_EARNED, OnEnemyDead);
            Messenger<int, int>.AddListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
            Messenger.AddListener(GameEvent.LEVEL_COMPLETED, OnLevelComplete);
            Messenger.AddListener(GameEvent.LEVEL_FAILED, OnLevelFailed);
            Messenger.AddListener(GameEvent.GAME_COMPLETED, OnGameComplete);
        }

        private void Start()
        {
            RefreshUI();
            OnScoreChanged(0);
        }

        private void OnDestroy()
        {
            Messenger.RemoveListener(GameEvent.SCORE_EARNED, OnEnemyDead);
            Messenger<int, int>.RemoveListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
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

        private void OnEnemyDead() => OnScoreChanged(scoreDelta);

        private void OnScoreChanged(int value)
        {
            if (!scoreLabelOn) return;
            score += value;
            scoreLabel.text = score.ToString();
            if (score >= scoresToWin)
            {
                OnLevelComplete();
            }
        }

        private void OnHealthUpdated(int health, int maxHealth)
        {
            if (!healthLabelOn) return;
            healthLabel.text = $"Health: {health} / {maxHealth}";
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
            Messenger<float>.Broadcast(GameEvent.SPEED_CHANGED, 0);
            yield return new WaitForSeconds(2);

            Managers.Managers.Mission.GoToNext();
        }

        private void OnLevelFailed() => StartCoroutine(FailLevel());

        private IEnumerator FailLevel()
        {
            levelEnding.gameObject.SetActive(true);
            levelEnding.text = levelFailedMessage;
            Messenger<float>.Broadcast(GameEvent.SPEED_CHANGED, 0);
            yield return new WaitForSeconds(2);

            Managers.Managers.Mission.RestartCurrent();
        }

        private void OnGameComplete()
        {
            levelEnding.gameObject.SetActive(true);
            levelEnding.text = gameCompleteMessage;
            Messenger<float>.Broadcast(GameEvent.SPEED_CHANGED, 0);
        }

        public void RestartGame()
        {
            OnOpenSettings();
            Managers.Managers.Mission.RestartGame();
        }

        public void RestartLevel()
        {
            OnOpenSettings();
            Managers.Managers.Mission.RestartCurrent();
        }

        public void SaveGame() => Managers.Managers.Data.SaveGameState();

        public void LoadGame()
        {
            OnOpenSettings();
            Managers.Managers.Data.LoadGameState();
        }

        public void Exit() => Application.Quit();
    }
}