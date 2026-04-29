using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Controls");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
