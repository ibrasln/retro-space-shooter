using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectController : MonoBehaviour
{
    public static SoundEffectController instance;

    public AudioSource[] soundEffects;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    public void SoundEffect(int chooseSound)
    {
        soundEffects[chooseSound].Stop();
        soundEffects[chooseSound].Play();
    }

    public void BulletSoundEffect(int chooseSound)
    {
        soundEffects[chooseSound].Stop();
        soundEffects[chooseSound].pitch = Random.Range(0.5f, 1f);
        soundEffects[chooseSound].Play();
    }
}
