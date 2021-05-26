using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Control Audio")]
    public AudioSource musicSource; // musica de fondo
    public AudioSource sfxSource; //efectos de sonido
    public float lowPitch = 0.95f;
    public float highPitch = 1.05f;
    public static SoundManager instance;

    void Awake()
    {
        if (SoundManager.instance == null)
        {
            SoundManager.instance = this;
        }else if(SoundManager.instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    //Reproducir efecto de sonido
    public void PlaySingle(AudioClip clip)
    {
        sfxSource.pitch = 1f;
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    public void RandomizeSfx(params AudioClip[] clips)
    {
        int index = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitch, highPitch);
        sfxSource.pitch = randomPitch;
        sfxSource.clip = clips[index];
        sfxSource.Play();
    }
}
