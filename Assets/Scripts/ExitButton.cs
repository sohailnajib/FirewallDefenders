using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void ExitGame()
    {
        Time.timeScale = 1f;
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
