using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
    [SerializeField] private Slider speedSlider;
    private float speed = 1;
        
    public void Open()
    {
        speedSlider.value = PlayerPrefs.GetFloat("Speed", 1);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        Messenger<float>.Broadcast(GameEvent.SPEED_CHANGED, speed);
        PlayerPrefs.SetFloat("Speed", speed);
        gameObject.SetActive(false);
    }

    public void OnSubmitName(string name)
    {
        Debug.Log(name);
    }

    public void OnSpeedValue(float speed)
    {
        this.speed = speed;
    }
}