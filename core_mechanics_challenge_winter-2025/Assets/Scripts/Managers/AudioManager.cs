using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

[System.Serializable]
public class AudioSFX
{
    public string name;
    public AudioClip audioClip;
    [Range(0, 1)] public float volume = 0.8f;
    public bool randomizePitch;
    public Vector2 randomizePitchValues = new(0.8f, 1.3f);
    public bool playOnAwake;
    public bool loop;
}

public class AudioManager : Singleton<AudioManager>
{
    [Header("Mixer Settings")]
    [SerializeField]
    private AudioMixer m_mixer;

    [SerializeField] private AudioMixerGroup m_sfxMixer;

    [Header("Audio SFX Settings")]
    [SerializeField]
    private List<AudioSFX> m_audioSFXList;

    private static List<GameObject> m_audioSourceObjects;

    protected override void Awake()
    {
        base.Awake();
        InitializeAudioSources();
    }

    // private void OnEnable()
    // {
    //     GameEvents.OnEnemyDestroyed += PlayEnemyKillSound;
    //     GameEvents.OnBombExplode += PlayExplosionSound;
    // }
    //
    // private void OnDisable()
    // {
    //     GameEvents.OnEnemyDestroyed -= PlayEnemyKillSound;
    //     GameEvents.OnBombExplode -= PlayExplosionSound;
    //
    // }

    private void InitializeAudioSources()
    {
        if (m_audioSFXList.Count > 0)
        {
            m_audioSourceObjects = new List<GameObject>();

            foreach (AudioSFX sfx in m_audioSFXList)
            {
                GameObject sfxObject = new GameObject(sfx.name, typeof(AudioSource));
                sfxObject.transform.SetParent(transform);

                AudioSource newAudioSource = sfxObject.GetComponent<AudioSource>();
                newAudioSource.volume = sfx.volume;
                newAudioSource.clip = sfx.audioClip;
                if (sfx.playOnAwake)
                {
                    newAudioSource.playOnAwake = sfx.playOnAwake;
                    newAudioSource.Play();
                }
                newAudioSource.loop = sfx.loop;
                newAudioSource.outputAudioMixerGroup = m_sfxMixer;
                m_audioSourceObjects.Add(sfxObject);
            }
        }
    }

    public AudioSource GetAudioSource(string _name)
    {
        foreach (GameObject obj in m_audioSourceObjects)
        {
            if (obj.name == _name)
                return obj.GetComponent<AudioSource>();
        }
        return null;
    }

    private AudioSFX GetAudioSFXByName(string _name)
    {
        foreach (AudioSFX sfx in m_audioSFXList)
        {
            if (sfx.name == _name)
                return sfx;
        }
        return null;
    }

    public void PlaySound(string _name)
    {
        AudioSFX sfxStats = GetAudioSFXByName(_name);
        AudioSource source = GetAudioSource(_name);
        if (source)
        {
            if (sfxStats.randomizePitch)
            {
                source.pitch = Random.Range(sfxStats.randomizePitchValues.x, sfxStats.randomizePitchValues.y);
            }
            source.Play();
        }
    }

    public bool IsPlaying(string _name)
    {
        return GetAudioSource(_name).isPlaying;
    }

    public void PlaySound(string _name, Vector3 _positon)
    {
        AudioSFX sfxStats = GetAudioSFXByName(_name);
        AudioSource source = GetAudioSource(_name);

        if (source)
        {
            if (sfxStats.randomizePitch)
            {
                source.pitch = Random.Range(sfxStats.randomizePitchValues.x, sfxStats.randomizePitchValues.y);
            }
            source.transform.position = _positon;
            source.spatialBlend = 1f;
            source.Play();
        }
    }

    public void StopSound(string _name)
    {
        AudioSource source = GetAudioSource(_name);

        if (source && source.isPlaying)
        {
            source.Stop();
        }
    }

    private void PlayEnemyKillSound()
    {
        PlaySound("FoodEaten");
    }

    private void PlayExplosionSound()
    {
        PlaySound("Explosion");
    }
}
