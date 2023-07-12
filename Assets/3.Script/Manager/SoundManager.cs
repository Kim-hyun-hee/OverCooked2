using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; // ∞Ó ¿Ã∏ß
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;
    public bool loop;

    [HideInInspector]
    public AudioSource source;
}


public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    public Sound[] sounds;
    private bool underOneMinute = false;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    void Start()
    {
        Play("Restaurant");
    }

    void Update()
    {
        //if (OrderManager.Instance.levelTime <= 60 && !underOneMinute)
        //{
        //    sounds[0].pitch = 1.5f;
        //    underOneMinute = true;
        //}
    }

    public void Play(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
            {
                sound.source.Play();
                break;
            }

        }
    }

    public void Stop(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
            {
                sound.source.Stop();
                break;
            }

        }
    }

    public bool IsPlaying(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
                return sound.source.isPlaying;
        }
        return false;
    }


}

