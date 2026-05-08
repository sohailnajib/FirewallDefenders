using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; 

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        StartCoroutine(LoadWithFade());
    }

    IEnumerator LoadWithFade()
    {
        MainMenuAudio audio = FindObjectOfType<MainMenuAudio>();
        if (audio != null)
        {
            yield return StartCoroutine(audio.FadeOutMusic());
        }
        
        SceneManager.LoadScene("GameScene");
    }
}