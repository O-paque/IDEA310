using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level 1 - Castle");
    }

    public void quit()
    {
        Application.Quit();
    }
}
