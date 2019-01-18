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
    private int score;

    private void Awake()
    {
        Messenger.AddListener(GameEvent.ENEMY_HIT, OnEnemyHit);
        Messenger.AddListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);

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

    private void Update()
    {
        if (Input.GetButtonDown("Cancel")) settingsPopup.OpenClose();
        if (Input.GetButtonDown("Inventory")) inventoryPopup.OpenClose();
    }
}