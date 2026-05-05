using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void Restart()
    {
        // Reset timescale
        Time.timeScale = 1f;
        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
