using UnityEngine;
using System.Collections;

public class GameSceneAudio : MonoBehaviour
{
    [Header("Wave Music")]
    public AudioClip waveMusic;
    public float musicVolume = 0.2f;

    [Header("Game Over Audio")]
    public AudioClip gameOverMusic;

    [Header("Bullet Audio")]
    public AudioClip bulletFireSound;
    public float bulletVolume = 1f;
    public float bulletPitchMin = 0.8f;
    public float bulletPitchMax = 1.2f;

    private AudioSource musicSource;
    private AudioSource bulletSource;
    private bool isGameOver = false;

    void Start()
    {
        // Separate AudioSource for music so bullet sounds don't interrupt it
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.playOnAwake = false;

        if (waveMusic != null)
        {
            musicSource.clip = waveMusic;
            musicSource.Play();
        }

        // Dedicated source for bullet sounds
        bulletSource = gameObject.AddComponent<AudioSource>();
        bulletSource.loop = false;
        bulletSource.volume = bulletVolume;
        bulletSource.playOnAwake = false;
    }

    public void PlayBulletSound()
    {
        if (isGameOver || bulletFireSound == null || bulletSource == null)
            return;

        // Randomise pitch slightly for variety
        bulletSource.pitch = Random.Range(bulletPitchMin, bulletPitchMax);
        bulletSource.PlayOneShot(bulletFireSound, bulletVolume);
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        musicSource.Stop();

        if (gameOverMusic != null)
        {
            musicSource.loop = false;
            musicSource.clip = gameOverMusic;
            musicSource.Play();
        }
    }
}
