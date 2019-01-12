using System;
using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour, IGameManager
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioSource musicS1Source;
        [SerializeField] private string introBGMusic;
        [SerializeField] private string levelBGMusic;
        
        public ManagerStatus status { get; private set; }
        private float volume = 0.5f;
        private bool soundMuted = false;
        
        public bool soundMute
        {
            get { return soundMuted; }
            set { AudioListener.pause = soundMuted = value; }
        }

        public float soundVolume
        {
            get { return volume; }
            set { AudioListener.volume = volume = value; }
        }

        private float musicVolume;
        public float MusicVolume
        {
            get { return musicVolume; }
            set
            {
                musicVolume = value;
                if (musicS1Source != null)
                {
                    musicS1Source.volume = musicVolume;
                }
            }
        }

        public bool MusicMute
        {
            get
            {
                if (musicS1Source != null)
                {
                    return musicS1Source.mute;
                }

                return false;
            }
            set
            {
                if (musicS1Source != null) musicS1Source.mute = value;
            }
        }

        public void PlayIntroMusic() => PlayMusic(Resources.Load("Music/" + introBGMusic) as AudioClip);
        public void PlayLevelMusic() => PlayMusic(Resources.Load("Music/" + levelBGMusic) as AudioClip);
        public void StopMusic() => musicS1Source.Stop();
        public void PlaySound(AudioClip audioClip) => audioSource.PlayOneShot(audioClip);

        private void PlayMusic(AudioClip audioClip)
        {
            musicS1Source.clip = audioClip;
            musicS1Source.Play();
        }

        public void Startup(NetworkService service)
        {
            Debug.Log("Audio manager starting...");
            soundVolume = PlayerPrefs.GetFloat("Volume", 0.5f);
            soundMute = Convert.ToBoolean(PlayerPrefs.GetInt("Muted", 0));
            MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
            MusicMute = Convert.ToBoolean(PlayerPrefs.GetInt("MusicMuted", 0));

            musicS1Source.ignoreListenerVolume = true;
            musicS1Source.ignoreListenerPause = true;
            PlayLevelMusic();
            status = ManagerStatus.Started;
        }

        private void OnDestroy()
        {
            PlayerPrefs.SetFloat("Volume", soundVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetInt("Muted", Convert.ToInt32(soundMute));
            PlayerPrefs.SetInt("MusicMuted", Convert.ToInt32(MusicMute));
        }
    }
}