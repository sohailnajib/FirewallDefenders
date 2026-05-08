using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void ExitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
