using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
	public void playButton() {
		SceneManager.LoadScene("Level 1 - Forest");
	}

	public void quitButton() {
		Application.Quit();
	}
}
