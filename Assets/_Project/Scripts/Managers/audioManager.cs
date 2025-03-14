using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections;

public class audioManager : MonoBehaviour
{
    public Sound[] bgmSounds;
    public Sound[] sounds;
    public static audioManager instance;
    public static float bgMusicVolume = .18f;
    public static float effectsMusicVolume = .18f;
    Sound actualBGM;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in bgmSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    private void Start()
    {
        PlayBGM("Principal");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("No se encontró el audio!");
            return;
        }
        s.source.Play();
    }
    public void PlayBGM(string name)
    {
        actualBGM = Array.Find(bgmSounds, bgmSounds => bgmSounds.name == name);
        if (actualBGM == null)
        {
            Debug.LogError("No se encontró el audio!");
            return;
        }
        actualBGM.source.Play();
    }
    public void updateBGMusic(string newTheme)
    {
        if (actualBGM.name != newTheme)
        {
            actualBGM.source.Stop();
            PlayBGM(newTheme);
            updateBGValume(bgMusicVolume);
        }
    }
    public void updateBGValume(float volume)
    {
        bgMusicVolume = volume;
        actualBGM.source.volume = volume;
    }
    public void updateEffectsVolume(float volume)
    {
        effectsMusicVolume = volume;
        foreach (Sound s in sounds)
        {
            s.source.volume = volume;
        }
    }
    private IEnumerator UpdateBGMusicWithFade(string newTheme, float fadeDuration)
    {
        float t = 0.0f;
        Sound newBGM = Array.Find(bgmSounds, bgmSounds => bgmSounds.name == newTheme);
        if (newBGM == null)
        {
            Debug.LogError("No se encontró el audio!");
            yield break;
        }

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalizedTime = t / fadeDuration;
            actualBGM.source.volume = Mathf.Lerp(bgMusicVolume, 0, normalizedTime);
            newBGM.source.volume = Mathf.Lerp(0, bgMusicVolume, normalizedTime);
            yield return null;
        }

        actualBGM.source.Stop();
        newBGM.source.volume = 0.5f;
        PlayBGM(newTheme);
    }
    
    public void PlayAtPosition(string name, Vector3 position)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("No se encontró el audio!");
            return;
        }
        
        // AudioSource.PlayClipAtPoint(s.clip, position);
        
        
        GameObject temp = new GameObject("TempAudio_" + name);
        temp.transform.position = position;
        AudioSource tempSource = temp.AddComponent<AudioSource>();
        
        tempSource.clip = s.clip;
        tempSource.volume = effectsMusicVolume;
        tempSource.pitch = s.pitch;
        tempSource.loop = false;
        tempSource.spatialBlend = 1f;
        
        tempSource.Play();

        Destroy(temp, s.clip.length);

    }
    
    public void Stop()
    {
        actualBGM.source.Stop();
    }
    public void Resume() {
        actualBGM.source.Play();
    }
    public void updateWithFade(string newTheme, float fadeDuration)
    {
        if (actualBGM.name != newTheme)
        {
            StartCoroutine(UpdateBGMusicWithFade(newTheme, fadeDuration));
        }
    }  

}
