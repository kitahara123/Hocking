using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreLabel;
    [SerializeField] private SettingsPopup settingsPopup;

    [SerializeField] private int scoreDelta;
    private int score;

    private void Awake()
    {
        Messenger.AddListener(GameEvent.ENEMY_HIT, OnEnemyHit);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.ENEMY_HIT, OnEnemyHit);
    }

    private void Start()
    {
        score = 0;
        scoreLabel.text = score.ToString();
    }

    public void OnOpenSettings()
    {
        if (settingsPopup.isActiveAndEnabled)
            settingsPopup.Close();
        else
            settingsPopup.Open();
    }

    public void OnPointerDown()
    {
//        Debug.Log("pointer down");
    }

    private void OnEnemyHit()
    {
        score += scoreDelta;
        scoreLabel.text = score.ToString();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel")) OnOpenSettings();
    }
}