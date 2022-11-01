using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    public AudioSource musicSource;
    public AudioSource soundSource;

    public AudioClip mainMenu, ingame, button, buyTower, sellTower, upgradeTower, gold, waveStart, lose, archerShoot, archerHit, canonShoot, canonHit, missileShoot, missileHit, magicShoot, magicHit, lightningShoot;

    private string soundKey = "sound";
    private int sound;
    public int Sound
    {
        get
        {
            if (!PlayerPrefs.HasKey(soundKey))
            {
                PlayerPrefs.SetInt(soundKey, 1);
            }

            sound = PlayerPrefs.GetInt(soundKey);

            return sound;
        }
        set
        {
            sound = value;
            PlayerPrefs.SetInt(soundKey, sound);

            SetSound();
        }
    }

    private string vibrateKey = "vibrate";
    private int vibrate;
    public int Vibrate
    {
        get
        {
            if (!PlayerPrefs.HasKey(vibrateKey))
            {
                PlayerPrefs.SetInt(vibrateKey, 1);
            }

            vibrate = PlayerPrefs.GetInt(vibrateKey);

            return vibrate;
        }
        set
        {
            vibrate = value;
            PlayerPrefs.SetInt(vibrateKey, vibrate);

            SetVibrate();
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetSound();
        SetVibrate();
        musicSource.clip = mainMenu;
        musicSource.Play();
    }

    void SetSound()
    {
        if (Sound == 0)
        {
            UIController.instance.soundOn.SetActive(false);
            UIController.instance.soundOff.SetActive(true);

            soundSource.volume = 0;
            musicSource.volume = 0;
        }
        else
        {
            UIController.instance.soundOn.SetActive(true);
            UIController.instance.soundOff.SetActive(false);

            soundSource.volume = 1;
            musicSource.volume = 1;
        }
    }

    void SetVibrate()
    {
        if (Vibrate == 0)
        {
            UIController.instance.vibrateOn.SetActive(false);
            UIController.instance.vibrateOff.SetActive(true);
        }
        else
        {
            UIController.instance.vibrateOn.SetActive(true);
            UIController.instance.vibrateOff.SetActive(false);
        }
    }

    public void PlaySound(string clip)
    {
        switch (clip)
        {
            case "button":
                soundSource.PlayOneShot(button);
                break;
            case "buyTower":
                soundSource.PlayOneShot(buyTower);
                break;
            case "sellTower":
                soundSource.PlayOneShot(sellTower);
                break;
            case "upgradeTower":
                soundSource.PlayOneShot(upgradeTower);
                break;
            case "gold":
                soundSource.PlayOneShot(gold);
                break;
            case "waveStart":
                soundSource.PlayOneShot(waveStart);
                break;
            case "lose":
                soundSource.PlayOneShot(lose);
                break;
            case "mainMenu":
                musicSource.clip = mainMenu;
                musicSource.Play();
                break;
            case "ingame":
                musicSource.clip = ingame;
                musicSource.Play();
                break;
            case "archerShoot":
                soundSource.PlayOneShot(archerShoot);
                break;
            case "archerHit":
                soundSource.PlayOneShot(archerHit);
                break;
            case "canonShoot":
                soundSource.PlayOneShot(canonShoot);
                break;
            case "canonHit":
                soundSource.PlayOneShot(canonHit);
                break;
            case "missileShoot":
                soundSource.PlayOneShot(missileShoot);
                break;
            case "missileHit":
                soundSource.PlayOneShot(missileHit);
                break;
            case "magicShoot":
                soundSource.PlayOneShot(magicShoot);
                break;
            case "magicHit":
                soundSource.PlayOneShot(magicHit);
                break;
            case "lightningShoot":
                soundSource.PlayOneShot(lightningShoot);
                break;
        }
    }

    
    public void PlayVibrate()
    {
        if (Vibrate != 0)
        {
            Vibrator.Vibrate(500);
        }
    }

}
