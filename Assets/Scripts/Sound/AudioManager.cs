using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSound, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("Theme", transform.position);
    }

    public void PlayMusic(string name, Vector3 pos)
    {
        Sound s = Array.Find(musicSound, x  => x.name == name);

        if(s== null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            musicSource.transform.position = pos;
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name, Vector3 pos)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            sfxSource.transform.position = pos;
            sfxSource.PlayOneShot(s.clip);
        }
    }
}
