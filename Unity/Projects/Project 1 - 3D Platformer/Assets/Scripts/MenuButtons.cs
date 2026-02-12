using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
	public void playButton() {
		SceneManager.LoadScene("ControlsScreen");
		Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}

	public void quitButton() {
		Application.Quit();
	}
}
