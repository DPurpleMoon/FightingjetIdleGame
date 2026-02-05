using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public GameObject manObj;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource movementLoopSource;

    [Header("Music Clips")]
    [SerializeField] private AudioClip startMenuMusic;
    [SerializeField] private AudioClip gameplayMusic;

    [Header("Player SFX")]
    [SerializeField] private AudioClip playerShoot;
    [SerializeField] private AudioClip playerLaser;
    [SerializeField] private AudioClip playerMovement;
    [SerializeField] private AudioClip playerMovement2;
    [SerializeField] private AudioClip playerBombing;
    [SerializeField] private AudioClip playerDeath;
    [SerializeField] private AudioClip playerTakeDamage;

    [Header("Enemy SFX")]
    [SerializeField] private AudioClip enemyTakeDamage;

    [Header("Volume Settings")]
    [SerializeField] [Range(0f, 1f)] private float musicVolume = 0.5f;
    [SerializeField] [Range(0f, 1f)] private float sfxVolume = 0.7f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
        object temp = SaveLoad.LoadGame("MusicVolume");
        if (temp != null)
        {
            musicVolume = (float)SaveLoad.LoadGame("MusicVolume");
        }
        else
        {
            musicVolume = 0.5f;
            SaveLoad.SaveGame("MusicVolume", musicVolume);
        }
        
        if (sfxVolume != null)
        {
            sfxVolume = (float)SaveLoad.LoadGame("SFXVolume");
        }
        else
        {
            sfxVolume = 0.5f;
            SaveLoad.SaveGame("SFXVolume", sfxVolume);
        }

        if (musicSource != null)
        {
            musicSource.loop = true;
            musicSource.volume = musicVolume;
        }

        if (sfxSource != null)
        {
            sfxSource.loop = false;
            sfxSource.volume = sfxVolume;
        }

        if (movementLoopSource != null)
        {
            movementLoopSource.loop = true;
            movementLoopSource.volume = sfxVolume;
            movementLoopSource.playOnAwake = false;
        }
    }

    #region Music Methods
    public void PlayStartMenuMusic()
    {
        PlayMusic(startMenuMusic);
    }

    public void PlayGameplayMusic()
    {
        PlayMusic(gameplayMusic);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (musicSource != null && clip != null)
        {
            if (musicSource.clip != clip)
            {
                musicSource.clip = clip;
                musicSource.Play();
            }
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
        Debug.Log("AudioManager: Music volume updated to " + musicVolume);
    }
    #endregion

    #region SFX Methods
    public void PlayPlayerShoot()
    {
        PlaySFX(playerShoot);
    }

    public void PlayPlayerLaser()
    {
        PlaySFX(playerLaser);
    }

    public void PlayPlayerMovement()
    {
        PlaySFX(playerMovement);
    }

    public void PlayPlayerMovement2()
    {
        PlaySFX(playerMovement2);
    }

    public void PlayPlayerMovementLoop(bool shouldPlay)
    {
        if (movementLoopSource == null) return;

        if (shouldPlay)
        {
            if (!movementLoopSource.isPlaying)
            {
                movementLoopSource.clip = playerMovement;
                movementLoopSource.Play();
            }
        }
        else
        {
            if (movementLoopSource.isPlaying)
            {
                movementLoopSource.Stop();
            }
        }
    }

    public void PlayPlayerBombing()
    {
        PlaySFX(playerBombing);
    }

    public void PlayPlayerDeath()
    {
        PlaySFX(playerDeath);
    }

    public void PlayPlayerTakeDamage()
    {
        PlaySFX(playerTakeDamage);
    }

    public void PlayEnemyTakeDamage()
    {
        PlaySFX(enemyTakeDamage);
    }

    private void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
        
        if (movementLoopSource != null)
        {
            movementLoopSource.volume = sfxVolume;
        }
        
        Debug.Log("AudioManager: SFX volume updated to " + sfxVolume);
    }
    #endregion
}