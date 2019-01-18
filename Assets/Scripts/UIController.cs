using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreLabel;
    [SerializeField] private TextMeshProUGUI healthLabel;
    [SerializeField] private SettingsPopup settingsPopup;
    [SerializeField] private InventoryPopup inventoryPopup;
    [SerializeField] private int scoreDelta;
    [SerializeField] private TextMeshProUGUI levelEnding;
    [SerializeField] private string levelCompleteMessage = "Level Complete!";
    [SerializeField] private string levelFailedMessage = "Level Failed";

    private int score;

    private void Awake()
    {
        Messenger.AddListener(GameEvent.ENEMY_HIT, OnEnemyHit);
        Messenger.AddListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
        Messenger.AddListener(GameEvent.LEVEL_COMPLETED, OnLevelComplete);
        Messenger.AddListener(GameEvent.LEVEL_FAILED, OnLevelFailed);

        if (!Managers.Managers.Settings.Isometric)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.ENEMY_HIT, OnEnemyHit);
        Messenger.RemoveListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
        Messenger.RemoveListener(GameEvent.LEVEL_COMPLETED, OnLevelComplete);
        Messenger.RemoveListener(GameEvent.LEVEL_FAILED, OnLevelFailed);
    }

    private void Start()
    {
        OnHealthUpdated();
        score = 0;
        scoreLabel.text = score.ToString();
    }

    public void OnOpenSettings() => settingsPopup.OpenClose();

    private void OnEnemyHit()
    {
        score += scoreDelta;
        scoreLabel.text = score.ToString();
    }

    private void OnHealthUpdated()
    {
        healthLabel.text = $"Health: {Managers.Managers.Player.health} / {Managers.Managers.Player.maxHealth}";
    }

    private void OnLevelComplete() => StartCoroutine(CompleteLevel());

    public IEnumerator CompleteLevel()
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

    private void Update()
    {
        if (Input.GetButtonDown("Cancel")) settingsPopup.OpenClose();
        if (Input.GetButtonDown("Inventory")) inventoryPopup.OpenClose();
    }
}