using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    private bool isShowing = false;

    private void Start()
    {
        ResetGameOverUI();
    }

    public void ShowGameOver()
    {
        Debug.Log("ShowGameOver called");

        if (isShowing)
            return;

        isShowing = true;

        if (gameOverPanel == null)
        {
            Debug.LogError("GameOverUI: gameOverPanel is null.");
            return;
        }

        gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResetGameOverUI()
    {
        isShowing = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void RestartScene()
    {
        ResetGameOverUI();

        if (HealthManager.Instance != null)
        {
            HealthManager.Instance.ResetAfterGameOver();
        }

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ClearInventory();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        ResetGameOverUI();

        if (HealthManager.Instance != null)
        {
            HealthManager.Instance.ResetAfterGameOver();
        }

        SceneManager.LoadScene("Main Menu");
    }
}