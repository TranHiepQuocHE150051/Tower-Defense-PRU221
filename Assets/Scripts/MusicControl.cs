using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
    public static MusicControl instance;

    [SerializeField] private AudioSource _musicSource, _effectsSource;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
                Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip) {
        _effectsSource.PlayOneShot(clip);
    }
    public void ChangeMasterVolume(float value)
    {
       AudioListener.volume= value; 
    }
}
