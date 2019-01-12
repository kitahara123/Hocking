using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private AudioClip clickSound;
    private float speed = 1;

    public void Open()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Messenger<float>.Broadcast(GameEvent.SPEED_CHANGED, 0f);
        speed = speedSlider.value = PlayerPrefs.GetFloat("Speed", 1);
        soundSlider.value = Managers.Managers.Audio.soundVolume;
        musicSlider.value = Managers.Managers.Audio.MusicVolume;
        soundToggle.isOn = Managers.Managers.Audio.soundMute;
        musicToggle.isOn = Managers.Managers.Audio.MusicMute;
        Managers.Managers.Audio.PlayIntroMusic();
        gameObject.SetActive(true);
    }

    public void Close()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Messenger<float>.Broadcast(GameEvent.SPEED_CHANGED, speed);
        PlayerPrefs.SetFloat("Speed", speed);
        Managers.Managers.Audio.PlayLevelMusic();
        gameObject.SetActive(false);
    }

    public void OnSubmitName(string value) => Debug.Log(value);
    public void OnSpeedValue(float value) => speed = value;
    public void OnSoundValue(float value) => Managers.Managers.Audio.soundVolume = value;

    public void OnSoundMuted(bool value)
    {
//        Managers.Managers.Audio.PlaySound(clickSound);
        Managers.Managers.Audio.soundMute = value;
    }

    public void OnMusicMute(bool value) => Managers.Managers.Audio.MusicMute = value;
    public void OnMusicValue(float value) => Managers.Managers.Audio.MusicVolume = value;
}