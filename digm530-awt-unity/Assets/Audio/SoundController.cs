using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour
{
    public AudioSource soundTrackSource;
    public AudioSource sfxSource;
    public AudioSource voiceOverSource;

    public AudioClip[] soundTracks;

    public static float SoundTrackVolume
    {
        get
        {
            return instance.soundTrackSource.volume;
        }
        set
        {
            instance.soundTrackSource.volume = value;
        }
    }
    public static float SFXVolume
    {
        get
        {
            return instance.sfxSource.volume;
        }
        set
        {
            instance.sfxSource.volume = value;
        }
    }
    public static float VOVolume
    {
        get
        {
            return instance.voiceOverSource.volume;
        }
        set
        {
            instance.voiceOverSource.volume = value;
        }
    }

    [Range(0.0f, 1.0f)]
    public float musicVolume = 1f;
    [Range(0.0f, 1.0f)]
    public float fxVolume = 1f;

    private static SoundController instance;
    void Awake ()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start ()
    {
        SoundTrackVolume = musicVolume;
        SFXVolume = fxVolume;
    }

    void Update ()
    {
        if (!soundTrackSource.isPlaying)
        {
            NextRandomSong();
        }
    }

    void NextRandomSong ()
    {
        if (soundTracks.Length == 0)
            return;
        soundTrackSource.clip = soundTracks[Random.Range(0, soundTracks.Length)];
        soundTrackSource.Play();
    }

    public static void PlaySFX (AudioClip clip)
    {
        PlaySFX(clip, 1f);
    }

    public static void PlaySFX (AudioClip clip, float volScale)
    {
        instance.sfxSource.PlayOneShot(clip, volScale);
    }

    public static void PlayVO (AudioClip vo)
    {
        PlayVO(vo, 1f);
    }

    public static void PlayVO (AudioClip vo, float volScale)
    {
        instance.voiceOverSource.PlayOneShot(vo, volScale);
    }

    public static void Initialize()
    {
        if(instance != null)
        {
            return;
        }
        GameObject go = Instantiate(Resources.Load("SoundController") as GameObject);
        instance = go.GetComponent<SoundController>();
    }
}
