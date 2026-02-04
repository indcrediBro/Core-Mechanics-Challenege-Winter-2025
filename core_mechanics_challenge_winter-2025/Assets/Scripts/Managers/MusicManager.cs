using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    public AudioSource mainMenuSource;
    public AudioSource inGameSource;
    public AudioClip mainMenuMusic;
    public AudioClip[] inGameMusicTracks;

    private int currentTrackIndex = 0;

    // private void OnEnable()
    // {
    //     GameEvents.OnGameStart += PlayNextInGameTrack;
    //     GameEvents.OnGameOver += StopAllMusic;
    // }
    // private void OnDisable()
    // {
    //     GameEvents.OnGameStart -= PlayNextInGameTrack;
    //     GameEvents.OnGameOver -= StopAllMusic;
    // }

    private void Start()
    {
    }

    private void Update()
    {
        if (!inGameSource.isPlaying && inGameSource.clip != null && !inGameSource.loop)
        {
            if (inGameSource.clip == mainMenuMusic)
            {
                return;
            }
            PlayNextInGameTrack();
        }
    }

    public void PlayMainMenuMusic()
    {
        PlayMusic(mainMenuMusic);
    }

    public void PlayNextInGameTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % inGameMusicTracks.Length;
        PlayMusic(inGameMusicTracks[currentTrackIndex]);
    }

    public void PlayInGameTrack(int trackIndex)
    {
        if (inGameSource.isPlaying) return;

        if (trackIndex >= 0 && trackIndex < inGameMusicTracks.Length)
        {
            currentTrackIndex = trackIndex;
            PlayMusic(inGameMusicTracks[currentTrackIndex]);
        }
        else
        {
            Debug.LogWarning("Invalid track index");
        }
    }
    public void PlayInGameTrack()
    {
        int trackIndex = Random.Range(0, inGameMusicTracks.Length);

        if (trackIndex >= 0 && trackIndex < inGameMusicTracks.Length)
        {
            currentTrackIndex = trackIndex;
            PlayMusic(inGameMusicTracks[currentTrackIndex]);
        }
        else
        {
            Debug.LogWarning("Invalid track index");
        }
    }

    public void StopAllMusic()
    {
        if (inGameSource.isPlaying)
        {
            inGameSource.Stop();
        }
        if (mainMenuSource.isPlaying)
        {
            mainMenuSource.Stop();
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        if (inGameSource.isPlaying)
        {
            inGameSource.Stop();
        }
        if (mainMenuSource.isPlaying)
        {
            mainMenuSource.Stop();
        }

        if (clip == mainMenuMusic)
        {
            mainMenuSource.clip = clip;
            mainMenuSource.Play();
        }
        else
        {
            inGameSource.clip = clip;
            inGameSource.Play();
        }
    }
}
