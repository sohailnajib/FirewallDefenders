using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuAudio : MonoBehaviour
{
    public AudioClip menuMusic;
    public float volume = 0.5f;
    public float fadeInDuration = 2f;
    public float fadeOutDuration = 1.5f;
    
    private AudioSource audioSource;
    private static MainMenuAudio instance;
    
    void Awake()
    {
        // Singleton pattern to keep music playing during scene transitions
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = menuMusic;
        audioSource.loop = true;
        audioSource.volume = 0f; // Start at 0 for fade in
        audioSource.Play();
        
        // Start fade in
        StartCoroutine(FadeInMusic());
        
        // Listen for scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    IEnumerator FadeInMusic()
    {
        float elapsed = 0;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, volume, elapsed / fadeInDuration);
            yield return null;
        }
        audioSource.volume = volume;
    }
    
    public IEnumerator FadeOutMusic()
    {
        float elapsed = 0;
        float startVolume = audioSource.volume;
        
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, elapsed / fadeOutDuration);
            yield return null;
        }
        audioSource.volume = 0;
        audioSource.Stop();
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If we're no longer in the main menu, fade out and destroy
        if (scene.name != "MainMenu")
        {
            StartCoroutine(FadeOutAndDestroy());
        }
    }
    
    IEnumerator FadeOutAndDestroy()
    {
        yield return StartCoroutine(FadeOutMusic());
        Destroy(gameObject);
    }
    
    void OnDestroy()
    {
        // Clean up the event listener
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}