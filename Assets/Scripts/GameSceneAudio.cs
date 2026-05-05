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
        // Setup music source
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.playOnAwake = false;
        
        if (waveMusic != null)
        {
            musicSource.clip = waveMusic;
            musicSource.Play();
            Debug.Log("Wave music started");
        }
        
        // Setup dedicated bullet source (completely separate from music)
        bulletSource = gameObject.AddComponent<AudioSource>();
        bulletSource.loop = false;
        bulletSource.volume = bulletVolume;
        bulletSource.playOnAwake = false;
        
        Debug.Log("GameSceneAudio initialized - Music and Bullet sources are separate");
    }
    
    public void PlayBulletSound()
    {
        Debug.Log("PlayBulletSound method CALLED!");
        
        if (isGameOver)
        {
            Debug.Log("Game is over, not playing bullet sound");
            return;
        }
        
        if (bulletFireSound == null)
        {
            Debug.LogError("Bullet sound clip not assigned in Inspector!");
            return;
        }
        
        if (bulletSource == null)
        {
            Debug.LogError("BulletSource is null!");
            return;
        }
        
        // Play sound on dedicated bullet source - won't affect music
        bulletSource.pitch = Random.Range(bulletPitchMin, bulletPitchMax);
        bulletSource.PlayOneShot(bulletFireSound, bulletVolume);
        Debug.Log("Bullet sound played at volume: " + bulletVolume);
    }
    
    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        
        Debug.Log("TriggerGameOver called");
        
        musicSource.Stop();
        
        if (gameOverMusic != null)
        {
            musicSource.loop = false;
            musicSource.clip = gameOverMusic;
            musicSource.Play();
            Debug.Log("Game over music started");
        }
    }
}