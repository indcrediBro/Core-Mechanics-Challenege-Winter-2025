using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : Singleton<SettingsManager>
{
    [Header("Audio Settings")]
    public AudioMixer audioMixer;            // Reference to the AudioMixer
    public string musicVolumeParameter = "MusicVolume";
    public string sfxVolumeParameter = "SFXVolume";

    private float musicVolume;
    private float sfxVolume;

    [Header("UI Settings")]
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    public Sprite switchIcon, switchIconFlipped;
    public Image cameraShakeButtonImageRenderer, screenFlashButtonImageRenderer;
    public bool cameraShakeEnabled, screenFlashEnabled;

    private void Start()
    {
        InitializeUI();
        LoadSettings();
    }

    private void InitializeUI()
    {
        musicVolumeSlider.value = GetMusicVolume();
        sfxVolumeSlider.value = GetSFXVolume();
        cameraShakeEnabled = GetCameraShakeActiveness() != 0;
        screenFlashEnabled = GetScreenFlashActiveness() != 0;

        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

        cameraShakeButtonImageRenderer.GetComponent<Button>().onClick.AddListener(SetCameraShakeActiveness);
        screenFlashButtonImageRenderer.GetComponent<Button>().onClick.AddListener(SetScreenFlashActiveness);
        
        if (cameraShakeEnabled)
        {
            cameraShakeButtonImageRenderer.sprite = switchIcon;
        }
        else
        {
            cameraShakeButtonImageRenderer.sprite = switchIconFlipped;
        }

        if (screenFlashEnabled)
        {
            screenFlashButtonImageRenderer.sprite = switchIcon;
        }
        else
        {
            screenFlashButtonImageRenderer.sprite = switchIconFlipped;
        }
    }

    private void OnMusicVolumeChanged(float value)
    {
        SetMusicVolume(value);
        ApplySettings();
    }

    private void OnSFXVolumeChanged(float value)
    {
        SetSFXVolume(value);
        ApplySettings();
    }

    private void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Log10(value) * 20;
        audioMixer.SetFloat(musicVolumeParameter, musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    private float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat("MusicVolume", 0.5f);
    }

    private void SetSFXVolume(float value)
    {
        sfxVolume = Mathf.Log10(value) * 20;
        audioMixer.SetFloat(sfxVolumeParameter, sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", value);
        AudioManager.Instance.PlaySound("MouseOver");
    }

    private float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat("SFXVolume", 0.5f);
    }

    public int GetCameraShakeActiveness()
    {
        return PlayerPrefs.GetInt("CameraShake", 1);
    }
    public int GetScreenFlashActiveness()
    {
        return PlayerPrefs.GetInt("ScreenFlash", 1);
    }
    public void SetCameraShakeActiveness()
    {
        if (cameraShakeEnabled)
        {
            PlayerPrefs.SetInt("CameraShake", 0);
            cameraShakeEnabled = false;
            cameraShakeButtonImageRenderer.sprite = switchIconFlipped;
        }
        else
        {
            PlayerPrefs.SetInt("CameraShake", 1);
            cameraShakeEnabled = true;
            cameraShakeButtonImageRenderer.sprite = switchIcon;
        }
        AudioManager.Instance.PlaySound("MouseOver");
    }
    public void SetScreenFlashActiveness()
    {
        if (screenFlashEnabled)
        {
            PlayerPrefs.SetInt("ScreenFlash", 0);
            screenFlashEnabled = false;
            screenFlashButtonImageRenderer.sprite = switchIconFlipped;
        }
        else
        {
            PlayerPrefs.SetInt("ScreenFlash", 1);
            screenFlashEnabled = true;
            screenFlashButtonImageRenderer.sprite = switchIcon;
        }
        AudioManager.Instance.PlaySound("MouseOver");
    }

    private void LoadSettings()
    {
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.5f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 0.5f));
    }

    private void ApplySettings()
    {
        audioMixer.SetFloat(musicVolumeParameter, Mathf.Log10(GetMusicVolume()) * 20);
        audioMixer.SetFloat(sfxVolumeParameter, Mathf.Log10(GetSFXVolume()) * 20);
    }
}
