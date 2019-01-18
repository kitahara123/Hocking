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
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
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
        saveButton.gameObject.SetActive(true);
        loadButton.gameObject.SetActive(true);
    }

    public void Close()
    {
        if (!Managers.Managers.Settings.Isometric)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; 
        }

        Messenger<float>.Broadcast(GameEvent.SPEED_CHANGED, speed);
        PlayerPrefs.SetFloat("Speed", speed);
        Managers.Managers.Audio.PlayLevelMusic();
        gameObject.SetActive(false);
        saveButton.gameObject.SetActive(false);
        loadButton.gameObject.SetActive(false);
    }
    
    public void OpenClose()
    {
        if (isActiveAndEnabled) Close();
        else Open();
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
    public void OnIsometricEnabled(bool value) => Managers.Managers.Settings.IsometricToggle(value);
}